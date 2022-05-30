using AutonomyForum.Middlewares;
using Microsoft.AspNetCore.CookiePolicy;

namespace AutonomyForum.Extentions;

public static class WebApplicationExtentions
{
    public static void ConfigureCors(this WebApplication app)
        => app.UseCors(builder => builder.WithOrigins("http://localhost:3000", "https://dykeml.github.io", "https://dykeml.github.io/AutonomyForumFront")
                                         .AllowAnyHeader()
                                         .WithMethods("GET", "PUT", "PATCH", "POST", "DELETE", "OPTIONS", "HEAD")
                                         .AllowCredentials()
                                         .SetIsOriginAllowed(_ => true));

    public static void ConfigureCookies(this WebApplication app)
        => app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.None,
            HttpOnly = HttpOnlyPolicy.None,
            Secure = CookieSecurePolicy.Always
        });

    public static void UseJwtCookie(this WebApplication app)
        => app.UseMiddleware<JwtCookieMiddleware>();
}