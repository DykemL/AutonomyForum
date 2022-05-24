namespace AutonomyForum.Services.Auth;

public interface IAuthService
{
    Task<AuthInfo?> LoginAsync(string userName, string password);
    Task<RegisterStatus> RegisterAsync(string userName, string email, string password, params string[] roles);
    Task<AuthInfo?> RefreshAsync(string refreshToken);
}