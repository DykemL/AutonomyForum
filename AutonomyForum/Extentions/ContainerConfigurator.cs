using System.IdentityModel.Tokens.Jwt;
using AutonomyForum.Models;
using AutonomyForum.Repositories;
using AutonomyForum.Services;
using AutonomyForum.Services.Auth;

namespace AutonomyForum.Extentions;

public static class ContainerConfigurator
{
    public static void Configure(IServiceCollection services)
    {
        services.AddSingleton<JwtSecurityTokenHandler>();

        services.AddTransient<IDbInitializer, AppDbInitializer>();

        ConfigureRepositories(services);
        ConfigureServices(services);
    }

    private static void ConfigureRepositories(IServiceCollection services)
        => services.AddScoped<UsersRepository>()
                   .AddScoped<SectionsRepository>()
                   .AddScoped<TopicsRepository>()
                   .AddScoped<RepliesRepository>();

    private static void ConfigureServices(IServiceCollection services)
        => services.AddScoped<IAuthService, AuthService>()
                   .AddScoped<UserService>()
                   .AddScoped<IJwtSecurityService, JwtSecurityService>()
                   .AddScoped<SectionsService>()
                   .AddScoped<TopicsService>()
                   .AddScoped<RepliesService>();
}