using AutonomyForum.Api.Types.Requests;
using AutonomyForum.Extentions;
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

    [HttpPost]
    public async Task<ActionResult<AuthInfo>> AuthAsync(AuthRequest authRequest)
    {
        var authInfo = await authService.AuthAsync(authRequest);
        if (authInfo == null)
        {
            return Unauthorized();
        }

        Response.Cookies.SetJwtToken(authInfo);
        return Ok(authInfo);
    }

    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterRequest model)
    {
        var registerStatus = await authService.RegisterAsync(model, AppRoles.User);
        if (registerStatus == RegisterStatus.AlreadyExists)
        {
            return BadRequest("Пользователь уже существует");
        }

        if (registerStatus == RegisterStatus.Error)
        {
            return BadRequest("Не удалось зарегистрировать пользователя");
        }

        if (registerStatus == RegisterStatus.Success)
        {
            return Ok("Пользователь успешно зарегистрирован");
        }

        return BadRequest("Неизвестная ошибка");
    }

    [Route("Refresh")]
    [HttpPost]
    public async Task<ActionResult<AuthInfo>> RefreshAsync(string refreshToken)
    {
        var authInfo = await authService.RefreshAsync(refreshToken);
        if (authInfo == null)
        {
            return Unauthorized();
        }

        Response.Cookies.SetJwtToken(authInfo);
        return Ok(authInfo);
    }
}