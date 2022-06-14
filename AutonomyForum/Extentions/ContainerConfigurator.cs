using System.IdentityModel.Tokens.Jwt;
using AutonomyForum.Models;
using AutonomyForum.Repositories;
using AutonomyForum.Services;
using AutonomyForum.Services.Auth;
using AutonomyForum.Services.Elections;

namespace AutonomyForum.Extentions;

public static class ContainerConfigurator
{
    public static void Configure(IServiceCollection services)
    {
        services.AddSingleton<JwtSecurityTokenHandler>();

        services.AddSingleton<IDbInitializer, AppDbInitializer>();

        ConfigureRepositories(services);
        ConfigureServices(services);
    }

    private static void ConfigureRepositories(IServiceCollection services)
        => services.AddScoped<UsersRepository>()
                   .AddScoped<SectionsRepository>()
                   .AddScoped<TopicsRepository>()
                   .AddScoped<RepliesRepository>()
                   .AddScoped<FilesRepository>()
                   .AddScoped<ElectionsRepository>()
                   .AddScoped<VotesRepository>()
                   .AddScoped<PrivateMessagesRepository>();

    private static void ConfigureServices(IServiceCollection services)
        => services.AddScoped<IAuthService, AuthService>()
                   .AddScoped<UserService>()
                   .AddScoped<IJwtSecurityService, JwtSecurityService>()
                   .AddScoped<SectionsService>()
                   .AddScoped<TopicsService>()
                   .AddScoped<RepliesService>()
                   .AddScoped<FilesService>()
                   .AddScoped<ElectionsService>()
                   .AddScoped<PrivateMessagesService>();
}