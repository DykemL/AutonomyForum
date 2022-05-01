using AutonomyForum.Middlewares;
using Microsoft.AspNetCore.CookiePolicy;

namespace AutonomyForum.Extentions;

public static class WebApplicationExtentions
{
    public static void ConfigureCors(this WebApplication app)
        => app.UseCors(x => x
                            .AllowAnyOrigin()
                            //.WithOrigins("https://localhost:3000")
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader());

    public static void ConfigureCookies(this WebApplication app)
        => app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

    public static void UseCookieJwt(this WebApplication app)
        => app.UseMiddleware<CookieJwtMiddleware>();
}