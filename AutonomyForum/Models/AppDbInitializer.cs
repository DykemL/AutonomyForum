using System.Security.Claims;
using AutonomyForum.Api.Controllers.Auth;
using AutonomyForum.Models.DbEntities;
using AutonomyForum.Services.Auth;
using AutonomyForum.Services.Claims.Permissions;
using AutonomyForum.Services.Roles;
using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Models;

public class AppDbInitializer : IDbInitializer
{
    private const string DefaultPassword = "123";

    private readonly IServiceScope scope;
    private readonly IAuthService authService;
    private readonly RoleManager<Role> roleManager;
    private readonly UserManager<User> userManager;
    private readonly AppSettings appSettings;

    public AppDbInitializer(IServiceProvider serviceProvider, AppSettings appSettings)
    {
        scope = serviceProvider.CreateScope();
        authService = scope.ServiceProvider.GetService<IAuthService>()!;
        roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>()!;
        userManager = scope.ServiceProvider.GetService<UserManager<User>>()!;
        this.appSettings = appSettings;
    }

    public async Task InitializeAsync()
    {
        await InitializeRolesAsync();
        await InitializeServiceUsersAsync();
    }

    private async Task InitializeRolesAsync()
    {
        await TryAddRoleAsync(AppRoles.User);
        await TryAddRoleAsync(AppRoles.Banned, new[]
        {
            PermissionsGenerator.GenerateBaseClaim(Permissions.AllRestricted)
        });
        await TryAddRoleAsync(AppRoles.Admin, new[]
        {
            PermissionsGenerator.GenerateBaseClaim(Permissions.Ban),
            PermissionsGenerator.GenerateBaseClaim(Permissions.SetModerator),

            PermissionsGenerator.GenerateBaseClaim(Permissions.CreateSection),
            PermissionsGenerator.GenerateBaseClaim(Permissions.DeleteSection),
            PermissionsGenerator.GenerateBaseClaim(Permissions.ModifySection),
            PermissionsGenerator.GenerateBaseClaim(Permissions.DeleteTopic),
            PermissionsGenerator.GenerateBaseClaim(Permissions.DeleteReply)
        });
        await TryAddRoleAsync(AppRoles.Moderator, new[]
        {
            PermissionsGenerator.GenerateBaseClaim(Permissions.Ban),

            PermissionsGenerator.GenerateBaseClaim(Permissions.CreateSection),
            PermissionsGenerator.GenerateBaseClaim(Permissions.DeleteReply)
        });
    }

    private async Task InitializeServiceUsersAsync()
    {
        var admin = await userManager.FindByNameAsync("admin");
        if (admin == null)
        {
            await authService.Register("admin", "admin@autonomyforum", appSettings.AdminPassword ?? DefaultPassword, AppRoles.Admin);
        }
        var moderator = await userManager.FindByNameAsync("moderator");
        if (moderator == null)
        {
            await authService.Register("moderator", "moderator@autonomyforum", appSettings.ModeratorPassword ?? DefaultPassword, AppRoles.Moderator);
        }
    }

    private async Task TryAddRoleAsync(string roleName, Claim[]? claims = null)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new Role(roleName);
            await roleManager.CreateAsync(role);
        }

        if (claims == null)
        {
            return;
        }
        var roleClaims = await roleManager.GetClaimsAsync(role);
        foreach (var claim in claims)
        {
            if (roleClaims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            {
                continue;
            }

            await roleManager.AddClaimAsync(role, claim);
        }
    }

    public void Dispose()
        => scope.Dispose();
}