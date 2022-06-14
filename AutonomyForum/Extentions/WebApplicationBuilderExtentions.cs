using AutonomyForum.HostedServices;
using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AutonomyForum.Extentions;

public static class WebApplicationBuilderExtentions
{
    public static AppSettings ConfigureApplicationSettings(this WebApplicationBuilder builder)
    {
        var settings = AppSettings.CreateFrom(builder.Configuration);
        builder.Services.AddSingleton(settings);
        return settings;
    }

    public static void ConfigureDatabase(this WebApplicationBuilder builder, AppSettings settings)
        => builder.Services
                  .AddDbContext<AppDbContext>(options => options.UseNpgsql(settings.DatabaseConnectionString!))
                  .AddHostedService<DbInitializerHostedService>()
                  .AddHostedService<ElectionsHostedService>();

    public static void ConfigureIdentity(this WebApplicationBuilder builder)
        => builder.Services.AddIdentity<User, Role>(options =>
                  {
                      options.Password.RequireDigit = false;
                      options.Password.RequireLowercase = false;
                      options.Password.RequireUppercase = false;
                      options.Password.RequiredLength = 0;
                      options.Password.RequireNonAlphanumeric = false;
                  })
                  .AddEntityFrameworkStores<AppDbContext>()
                  .AddDefaultTokenProviders();

    public static void ConfigureJwt(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(appSettings.JwtSigningSecretKey)
            };
        });
    }

    public static void ConfigureCors(this WebApplicationBuilder builder)
        => builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(x =>
                                         x.SetIsOriginAllowed(_ => true)
                                          .AllowAnyMethod()
                                          .AllowAnyHeader()
                                          .AllowCredentials());
        });

    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureSwaggerGen(options =>
        {
            options.DescribeAllParametersInCamelCase();
        });
    }

    public static void ConfigureContainer(this WebApplicationBuilder builder)
        => ContainerConfigurator.Configure(builder.Services);
}