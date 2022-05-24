using System.Security.Claims;
using AutonomyForum.Models.DbEntities;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;
using AutonomyForum.Services.Claims.Permissions;
using AutonomyForum.Services.Roles;
using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Services;

public class UserService
{
    private readonly UsersRepository usersRepository;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;

    public UserService(UsersRepository usersRepository, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        this.usersRepository = usersRepository;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task UpdateUserAsync(User user)
        => await usersRepository.UpdateUserAsync(user);

    public async Task<User?> FindUserByIdAsync(Guid id)
        => await usersRepository.FindUserByIdAsync(id);

    public async Task<User?> FindUserByUserNameAsync(string userName)
        => await userManager.FindByNameAsync(userName);

    public async Task<User?> FindUserByRefreshTokenAsync(string refreshToken)
        => await usersRepository.FindUserByRefreshTokenAsync(refreshToken);

    public async Task<UserExtended?> GetUserExtendedAsync(Guid userId)
    {
        var existingUser = await FindUserByIdAsync(userId);
        if (existingUser == null)
        {
            return null;
        }

        var user = new UserExtended
        {
            Id = existingUser.Id,
            UserName = existingUser.UserName,
            Email = existingUser.Email,
            Roles = await GetUserRoles(userId)
        };
        user.Permissions = await GetClearedPermissionsAsync(user.UserName);

        return user;
    }

    public async Task<string[]> GetUserRoles(Guid userId)
    {
        var user = await FindUserByIdAsync(userId);
        if (user == null)
        {
            return new string[] {};
        }

        var roles = await userManager.GetRolesAsync(user);
        return roles.ToArray();
    }

    public async Task<string[]> GetClearedPermissionsAsync(string userName)
    {
        var claims = await GetPermissionClaimsAsync(userName);
        return claims.Select(x => x.Value.Replace(PermissionPrefixes.Base + ":", string.Empty)).ToArray();
    }

    public async Task<Claim[]> GetPermissionClaimsAsync(string userName)
    {
        var user = await FindUserByUserNameAsync(userName);
        if (user == null)
        {
            return new Claim[] {};
        }

        var userClaims = await userManager.GetClaimsAsync(user);
        var userRoles = await userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        foreach (var roleString in userRoles)
        {
            var role = await roleManager.FindByNameAsync(roleString);
            var claims = await roleManager.GetClaimsAsync(role);
            roleClaims.AddRange(claims);
        }

        return userClaims.Concat(roleClaims).ToArray();
    }

    public async Task<ModifyUserRoleStatus> AddRoleToUser(Guid currentUserId, Guid userId, string role)
    {
        var currentUser = await GetUserExtendedAsync(currentUserId);
        var user = await GetUserExtendedAsync(userId);
        if (user == null || currentUser == null)
        {
            return ModifyUserRoleStatus.UserIsNotExists;
        }
        if (!CheckModifyRolePermission(currentUser, user, role))
        {
            return ModifyUserRoleStatus.HasntPermission;
        }

        if (!await usersRepository.AddRoleToUser(userId, role))
        {
            return ModifyUserRoleStatus.Error;
        }

        return ModifyUserRoleStatus.Success;
    }

    public async Task<ModifyUserRoleStatus> RemoveRoleFromUser(Guid currentUserId, Guid userId, string role)
    {
        var currentUser = await GetUserExtendedAsync(currentUserId);
        var user = await GetUserExtendedAsync(userId);
        if (user == null || currentUser == null)
        {
            return ModifyUserRoleStatus.UserIsNotExists;
        }
        if (!CheckModifyRolePermission(currentUser, user, role))
        {
            return ModifyUserRoleStatus.HasntPermission;
        }

        if (!await usersRepository.RemoveRoleFromUser(userId, role))
        {
            return ModifyUserRoleStatus.Error;
        }

        return ModifyUserRoleStatus.Success;
    }

    private bool CheckModifyRolePermission(UserExtended currentUser, UserExtended targetUser, string role)
    {
        if (currentUser.Roles.Contains(AppRoles.Moderator) && targetUser.Roles.Contains(AppRoles.Moderator))
        {
            return false;
        }

        return currentUser.Permissions.Any(x => CheckPermissionToSetRole(role, x)) &&
               !targetUser.Roles.Contains(AppRoles.Admin);
    }

    private bool CheckPermissionToSetRole(string role, string permission)
        => role switch
        {
            AppRoles.Banned => permission == Permissions.Ban,
            AppRoles.Moderator => permission == Permissions.SetModerator,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
}