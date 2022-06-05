namespace AutonomyForum.Services.Auth;

public interface IAuthService
{
    Task<AuthInfo?> Login(string userName, string password);
    Task<RegisterStatus> Register(string userName, string email, string password, params string[] roles);
    Task<AuthInfo?> Refresh(string refreshToken);
}