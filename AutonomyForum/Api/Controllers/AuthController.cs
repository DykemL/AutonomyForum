using AutonomyForum.Api.Controllers.Auth;
using AutonomyForum.Extentions;
using AutonomyForum.Helpers;
using AutonomyForum.Services.Auth;
using AutonomyForum.Services.Roles;
using BankServer.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(HttpHeaders.JsonContentHeader)]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
        => this.authService = authService;

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
    {
        var authInfo = await authService.LoginAsync(loginRequest);
        if (authInfo == null)
        {
            return Unauthorized();
        }

        Response.Cookies.SetJwtToken(authInfo);
        return Ok();
    }

    [Route("Register")]
    [HttpPost]
    public async Task<ActionResult<RegisterResponse>> RegisterAsync([FromBody] RegisterRequest model)
    {
        var registerStatus = await authService.RegisterAsync(model, AppRoles.Admin);
        if (registerStatus == RegisterStatus.AlreadyExists)
        {
            return BadRequest(new RegisterResponse { Message = "Пользователь уже существует" });
        }

        if (registerStatus == RegisterStatus.Error)
        {
            return BadRequest(new RegisterResponse { Message = "Не удалось зарегистрировать пользователя" });
        }

        if (registerStatus == RegisterStatus.Success)
        {
            return Ok(new RegisterResponse { Message = "Пользователь успешно зарегистрирован" });
        }

        return BadRequest("Неизвестная ошибка");
    }

    [Route("Refresh")]
    [HttpPost]
    public async Task<IActionResult> RefreshAsync()
    {
        Request.Cookies.TryGetValue(CookieKeys.ApplicationRefreshToken, out var refreshToken);
        if (refreshToken == null)
        {
            return Unauthorized();
        }

        var authInfo = await authService.RefreshAsync(refreshToken);
        if (authInfo == null)
        {
            return Unauthorized();
        }

        Response.Cookies.SetJwtToken(authInfo);
        return Ok();
    }
}