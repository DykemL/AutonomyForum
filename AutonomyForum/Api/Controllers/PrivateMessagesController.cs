using AutonomyForum.Api.Controllers.PrivateMessages;
using AutonomyForum.Extentions;
using AutonomyForum.Helpers;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Produces(HttpHeaders.JsonContentHeader)]
[Authorize]
[Route("api/messages")]
public class PrivateMessagesController : ControllerBase
{
    private readonly PrivateMessagesService privateMessagesService;

    public PrivateMessagesController(PrivateMessagesService privateMessagesService)
        => this.privateMessagesService = privateMessagesService;

    [HttpGet]
    [Route("{collocutorId}")]
    public async Task<ActionResult<PrivateMessageInfo[]>> FindMessagesByReceiver([FromRoute] Guid collocutorId)
    {
        var messages = await privateMessagesService.GetPrivateMessages(User.GetId(), collocutorId);

        return Ok(messages);
    }

    [HttpGet]
    public async Task<ActionResult<UserInfo[]>> FindCollocutors()
    {
        var collocutors = await privateMessagesService.GetCollocutors(User.GetId());

        return Ok(collocutors);
    }

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult<PrivateMessageInfo[]>> AddPrivateMessageRequest([FromBody] AddPrivateMessageRequest request)
    {
        await privateMessagesService.AddMessage(User.GetId(), request.ReceiverId, request.Message);

        return Ok();
    }
}