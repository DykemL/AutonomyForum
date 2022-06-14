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

    public async Task UpdateUser(User user)
        => await usersRepository.UpdateUser(user);

    public async Task<User?> FindUserById(Guid id)
        => await usersRepository.FindUserById(id);

    public async Task<User?> FindUserByUserName(string userName)
        => await userManager.FindByNameAsync(userName);

    public async Task<User?> FindUserByRefreshToken(string refreshToken)
        => await usersRepository.FindUserByRefreshToken(refreshToken);

    public async Task<UserExtended?> GetUserExtended(Guid userId)
    {
        var existingUser = await FindUserById(userId);
        if (existingUser == null)
        {
            return null;
        }

        var user = new UserExtended
        {
            Id = existingUser.Id,
            UserName = existingUser.UserName,
            Email = existingUser.Email,
            Roles = await GetUserRoles(userId),
            AvatarFilePath = existingUser.AvatarFile?.Path,
            RepliesCount = existingUser.RepliesCount,
            Rating = existingUser.Rating
        };
        user.Permissions = await GetClearedPermissions(user.UserName);

        return user;
    }

    public async Task<string[]> GetUserRoles(Guid userId)
    {
        var user = await FindUserById(userId);
        if (user == null)
        {
            return new string[] {};
        }

        var roles = await userManager.GetRolesAsync(user);
        return roles.ToArray();
    }

    public async Task<string[]> GetClearedPermissions(string userName)
    {
        var claims = await GetPermissionClaims(userName);
        return claims.Select(x => x.Value.Replace(PermissionPrefixes.Base + ":", string.Empty)).Distinct().ToArray();
    }

    public async Task<Claim[]> GetPermissionClaims(string userName)
    {
        var user = await FindUserByUserName(userName);
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

    public async Task<bool> TryAttachAvatarToUser(Guid userId, Guid fileId)
        => await usersRepository.TryAttachAvatarToUser(userId, fileId);

    public async Task<ModifyUserRoleStatus> AddRoleToUser(Guid currentUserId, Guid userId, string role)
    {
        var currentUser = await GetUserExtended(currentUserId);
        var user = await GetUserExtended(userId);
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
        var currentUser = await GetUserExtended(currentUserId);
        var user = await GetUserExtended(userId);
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
        if (currentUser.Roles.Contains(AppRoles.Moderator) && targetUser.Roles.Contains(AppRoles.Moderator) ||
            currentUser.Roles.Contains(AppRoles.Prefect) && targetUser.Roles.Contains(AppRoles.Prefect) ||
            currentUser.Roles.Contains(AppRoles.Prefect) && targetUser.Roles.Contains(AppRoles.Moderator))
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
            AppRoles.Prefect => permission == Permissions.SetPrefect,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
}