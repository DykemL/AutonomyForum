using AutonomyForum.Middlewares;
using Microsoft.AspNetCore.CookiePolicy;

namespace AutonomyForum.Extentions;

public static class WebApplicationExtentions
{
    public static void ConfigureCors(this WebApplication app)
        => app.UseCors(builder => builder.WithOrigins("http://localhost:3000")
                                         .AllowAnyHeader()
                                         .AllowAnyMethod()
                                         .AllowCredentials());

    public static void ConfigureCookies(this WebApplication app)
        => app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.None,
            Secure = CookieSecurePolicy.Always
        });

    public static void UseJwtCookie(this WebApplication app)
        => app.UseMiddleware<JwtCookieMiddleware>();
}