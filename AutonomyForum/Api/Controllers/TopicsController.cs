using AutonomyForum.Api.Authorization;
using AutonomyForum.Api.Controllers.Topics;
using AutonomyForum.Extentions;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using AutonomyForum.Services.Claims.Permissions;
using BankServer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/topics")]
[Produces(HttpHeaders.JsonContentHeader)]
[Authorize]
public class TopicsController : ControllerBase
{
    private readonly TopicsService topicsService;

    public TopicsController(TopicsService topicsService)
        => this.topicsService = topicsService;

    [HttpPut]
    public async Task<ActionResult> CreateTopicAsync([FromBody] CreateTopicRequest request)
    {
        if (await topicsService.TryCreateTopicAsync(request.Title, request.TitleMessage, User.GetId(), request.SectionId))
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<TopicInfo>> FindTopicByIdAsync([FromRoute] FindTopicByIdRequest request)
    {
        var topicInfo = await topicsService.FindTopicAsync(request.Id);
        if (topicInfo == null)
        {
            return NotFound();
        }

        return Ok(topicInfo);
    }

    [HttpDelete]
    [RequirePermission(Permissions.DeleteTopic)]
    [Route("{id}")]
    public async Task<ActionResult> DeleteTopicAsync([FromRoute] Guid id)
    {
        await topicsService.DeleteTopicAsync(id);

        return Ok();
    }
}