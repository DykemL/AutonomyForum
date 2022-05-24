using AutonomyForum.Models.DbEntities.Types;

namespace AutonomyForum.Services.Auth;

public class AuthInfo
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; }
    public UserExtended UserExtended { get; set; }
}