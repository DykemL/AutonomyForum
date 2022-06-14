using AutonomyForum.Api.Authorization;
using AutonomyForum.Api.Controllers.Replies;
using AutonomyForum.Extentions;
using AutonomyForum.Helpers;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using AutonomyForum.Services.Claims.Permissions;
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
    public async Task<ActionResult> CreateReply([FromBody] CreateReplyRequest request)
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
    public async Task<ActionResult> DeleteReply([FromRoute] Guid id)
    {
        if (HttpContext.Items.ContainsKey(RequirePermissionAttribute.ConditionalCheckMarker))
        {
            if (!await repliesService.IsPrefect(id, User.GetId()))
            {
                return Forbid();
            }
        }
        await repliesService.DeleteReply(id);

        return Ok();
    }

    [HttpPost]
    [Route("{id}/like")]
    public async Task<ActionResult> DoLikeReply([FromRoute] Guid id)
    {
        var wasLiked = await repliesService.DoLikeReply(id, User.GetId());
        if (!wasLiked)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [Route("{id}/cancel-like")]
    public async Task<ActionResult> CancelLikeReply([FromRoute] Guid id)
    {
        var wasLiked = await repliesService.CancelLikeReply(id, User.GetId());
        if (!wasLiked)
        {
            return BadRequest();
        }

        return Ok();
    }
}