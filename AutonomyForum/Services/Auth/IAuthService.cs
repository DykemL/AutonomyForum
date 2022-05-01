using AutonomyForum.Api.Types.Requests;

namespace AutonomyForum.Services.Auth;

public interface IAuthService
{
    Task<AuthInfo?> AuthAsync(AuthRequest authRequest);
    Task<RegisterStatus> RegisterAsync(RegisterRequest registerRequest, params string[] roles);
    Task<AuthInfo?> RefreshAsync(string refreshToken);
}