using AutonomyForum.Api.Controllers.Auth;

namespace AutonomyForum.Services.Auth;

public interface IAuthService
{
    Task<AuthInfo?> LoginAsync(LoginRequest loginRequest);
    Task<RegisterStatus> RegisterAsync(RegisterRequest registerRequest, params string[] roles);
    Task<AuthInfo?> RefreshAsync(string refreshToken);
}