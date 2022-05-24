using AutonomyForum.Api.Controllers.Users;
using AutonomyForum.Extentions;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using BankServer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/users")]
[Produces(HttpHeaders.JsonContentHeader)]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService userService;

    public UsersController(UserService userService)
        => this.userService = userService;

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public async Task<ActionResult<UserExtended>> GetUserExtended([FromRoute] GetUserInfoRequest request)
    {
        var userExtended = await userService.GetUserExtendedAsync(request.Id);
        if (userExtended == null)
        {
            return NotFound();
        }

        return Ok(userExtended);
    }

    [HttpPost]
    [Route("{id}/add-role")]
    public async Task<ActionResult> AddRoleToUser([FromRoute] Guid id, [FromQuery] string role)
    {
        if (User.GetId() == id)
        {
            return BadRequest("Нельзя устанавливать роль самому себе");
        }

        var result = await userService.AddRoleToUser(User.GetId(), id, role);
        return result switch
        {
            ModifyUserRoleStatus.UserIsNotExists => BadRequest("Пользователь не существует"),
            ModifyUserRoleStatus.HasntPermission => BadRequest("Недостаточно прав"),
            ModifyUserRoleStatus.Error => BadRequest("Неизвестная ошибка при установке роли"),
            ModifyUserRoleStatus.Success => Ok(),
            _ => BadRequest("Неизвестная ошибка при установке роли")
        };
    }

    [HttpPost]
    [Route("{id}/remove-role")]
    public async Task<ActionResult> RemoveRoleFromUser([FromRoute] Guid id, [FromQuery] string role)
    {
        if (User.GetId() == id)
        {
            return BadRequest("Нельзя удалять роль самому себе");
        }

        var result = await userService.RemoveRoleFromUser(User.GetId(), id, role);
        return result switch
        {
            ModifyUserRoleStatus.UserIsNotExists => BadRequest("Пользователь не существует"),
            ModifyUserRoleStatus.HasntPermission => BadRequest("Недостаточно прав"),
            ModifyUserRoleStatus.Error => BadRequest("Неизвестная ошибка при удалении роли"),
            ModifyUserRoleStatus.Success => Ok(),
            _ => BadRequest("Неизвестная ошибка при удалении роли")
        };
    }
}