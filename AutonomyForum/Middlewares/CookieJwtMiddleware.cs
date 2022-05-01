using AutonomyForum.Helpers;

namespace AutonomyForum.Middlewares;

public class CookieJwtMiddleware
{
    private readonly RequestDelegate next;

    public CookieJwtMiddleware(RequestDelegate next) =>
        this.next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies[CookieKeys.ApplicationJwt];
        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers.Add("Authorization", "Bearer " + token);
        }

        await next(context);
    }
}