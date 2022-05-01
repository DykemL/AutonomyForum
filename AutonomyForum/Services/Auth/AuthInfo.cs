namespace AutonomyForum.Services.Auth;

public class AuthInfo
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}