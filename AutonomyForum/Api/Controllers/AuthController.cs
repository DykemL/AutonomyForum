using AutonomyForum.Api.Controllers.Auth;
using AutonomyForum.Extentions;
using AutonomyForum.Helpers;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using AutonomyForum.Services.Auth;
using AutonomyForum.Services.Claims.Permissions;
using AutonomyForum.Services.Roles;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/auth")]
[Produces(HttpHeaders.JsonContentHeader)]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly UserService userService;

    public AuthController(IAuthService authService, UserService userService)
    {
        this.authService = authService;
        this.userService = userService;
    }

    [Route("login")]
    [HttpPost]
    public async Task<ActionResult<UserExtended>> Login(LoginRequest request)
    {
        var authInfo = await authService.Login(request.UserName, request.Password);
        if (authInfo == null)
        {
            return Unauthorized();
        }

        if (authInfo.UserExtended.Permissions.Contains(Permissions.AllRestricted))
        {
            return Forbid();
        }

        Response.Cookies.SetJwtToken(authInfo);
        return Ok(authInfo.UserExtended);
    }

    [Route("register")]
    [HttpPost]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        var registerStatus = await authService.Register(request.UserName, request.Email, request.Password, AppRoles.User);
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

    [Route("refresh")]
    [HttpPost]
    public async Task<ActionResult<UserExtended>> Refresh()
    {
        Request.Cookies.TryGetValue(CookieKeys.ApplicationRefreshToken, out var refreshToken);
        if (refreshToken == null)
        {
            return Unauthorized();
        }

        var authInfo = await authService.Refresh(refreshToken);
        if (authInfo == null || authInfo.UserExtended.Permissions.Contains(Permissions.AllRestricted))
        {
            return Unauthorized();
        }

        Response.Cookies.SetJwtToken(authInfo);
        return Ok(authInfo.UserExtended);
    }

    [Route("current")]
    [HttpGet]
    public async Task<ActionResult<UserExtended>> GetCurrent()
    {
        var user = await userService.GetUserExtended(User.GetId());
        if (user == null || user.Permissions.Contains(Permissions.AllRestricted))
        {
            return Unauthorized();
        }

        return Ok(user);
    }
}