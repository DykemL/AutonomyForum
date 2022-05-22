using System.Security.Claims;
using AutonomyForum.Models.DbEntities;
using AutonomyForum.Services.Auth;
using AutonomyForum.Services.Claims.Permissions;
using AutonomyForum.Services.Roles;
using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Models;

public class AppDbInitializer : IDbInitializer
{
    private readonly IServiceScope scope;
    private readonly IAuthService authService;
    private readonly RoleManager<Role> roleManager;
    private readonly AppDbContext appDbContext;
    private readonly AppSettings appSettings;

    public AppDbInitializer(IServiceProvider serviceProvider, AppSettings appSettings)
    {
        scope = serviceProvider.CreateScope();
        authService = scope.ServiceProvider.GetService<IAuthService>()!;
        roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>()!;
        appDbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        this.appSettings = appSettings;
    }

    public async Task InitializeAsync()
        => await InitializeRolesAsync();

    private async Task InitializeRolesAsync()
    {
        await TryAddRoleAsync(AppRoles.User);
        await TryAddRoleAsync(AppRoles.Admin, new[]
        {
            PermissionsGenerator.GenerateBaseClaim(Permissions.CreateSection),
            PermissionsGenerator.GenerateBaseClaim(Permissions.DeleteSection),
            PermissionsGenerator.GenerateBaseClaim(Permissions.ModifySection)
        });
    }

    private async Task TryAddRoleAsync(string roleName, Claim[]? claims = null)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
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