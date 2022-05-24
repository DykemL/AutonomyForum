using AutonomyForum.Api.Authorization;
using AutonomyForum.Api.Controllers.Replies;
using AutonomyForum.Extentions;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using AutonomyForum.Services.Claims.Permissions;
using BankServer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/replies")]
[Produces(HttpHeaders.JsonContentHeader)]
[Authorize]
public class RepliesController : ControllerBase
{
    private readonly RepliesService repliesService;

    public RepliesController(RepliesService repliesService)
        => this.repliesService = repliesService;

    [HttpPut]
    public async Task<ActionResult> CreateReplyAsync([FromBody] CreateReplyRequest request)
    {
        var wasCreated = await repliesService.TryCreateReply(request.TopicId, User.GetId(), request.Message);
        if (!wasCreated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete]
    [RequirePermission(Permissions.DeleteReply)]
    [Route("{id}")]
    public async Task<ActionResult> DeleteReplyAsync([FromRoute] Guid id)
    {
        await repliesService.DeleteReplyAsync(id);

        return Ok();
    }

    [HttpPost]
    [Route("{id}/like")]
    public async Task<ActionResult> DoLikeReplyAsync([FromRoute] Guid id)
    {
        var wasLiked = await repliesService.DoLikeReply(id, User.GetId());
        if (!wasLiked)
        {
            return BadRequest();
        }

        return Ok();
    }
}