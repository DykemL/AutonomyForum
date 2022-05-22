using System.Security.Claims;
using AutonomyForum.Models.DbEntities;
using AutonomyForum.Repositories;
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

    public async Task<User?> FindUserByUserName(string userName)
        => await userManager.FindByNameAsync(userName);

    public async Task<User?> FindUserByRefreshToken(string refreshToken)
        => await usersRepository.FindUserByRefreshToken(refreshToken);

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
}