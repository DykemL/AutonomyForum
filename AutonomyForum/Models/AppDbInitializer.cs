using AutonomyForum.Models.DbEntities;
using AutonomyForum.Services.Auth;
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
        await TryAddRoleAsync(AppRoles.Admin);
    }

    private async Task TryAddRoleAsync(string roleName)
    {
        if (await roleManager.FindByNameAsync(roleName) == null)
        {
            await roleManager.CreateAsync(new Role(roleName));
        }
    }

    public void Dispose()
        => scope.Dispose();
}