using AutonomyForum.Helpers;
using AutonomyForum.Services.Auth;

namespace AutonomyForum.Extentions;

public static class ResponseCookiesExtentions
{
    private static readonly TimeSpan refreshTokenTtl = TimeSpan.FromDays(365); 

    public static void SetJwtToken(this IResponseCookies responseCookies, AuthInfo authInfo)
    {
        responseCookies.Append(CookieKeys.ApplicationJwt, authInfo.Token,
                               new CookieOptions
                               {
                                   SameSite = SameSiteMode.None,
                                   HttpOnly = false,
                                   MaxAge = authInfo.Expiration - DateTime.UtcNow
                               });
        responseCookies.Append(CookieKeys.ApplicationRefreshToken, authInfo.RefreshToken,
                               new CookieOptions
                               {
                                   SameSite = SameSiteMode.None,
                                   MaxAge = refreshTokenTtl
                               });
    }
}