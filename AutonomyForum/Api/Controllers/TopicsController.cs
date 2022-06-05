using AutonomyForum.Api.Authorization;
using AutonomyForum.Api.Controllers.Topics;
using AutonomyForum.Extentions;
using AutonomyForum.Helpers;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using AutonomyForum.Services.Claims.Permissions;
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
    public async Task<ActionResult> CreateTopic([FromBody] CreateTopicRequest request)
    {
        if (await topicsService.TryCreateTopic(request.Title, request.TitleMessage, User.GetId(), request.SectionId))
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<TopicInfo>> FindTopicById([FromRoute] FindTopicByIdRequest request)
    {
        var topicInfo = await topicsService.FindTopic(request.Id);
        if (topicInfo == null)
        {
            return NotFound();
        }

        return Ok(topicInfo);
    }

    [HttpDelete]
    [RequirePermission(Permissions.DeleteTopic)]
    [Route("{id}")]
    public async Task<ActionResult> DeleteTopic([FromRoute] Guid id)
    {
        await topicsService.DeleteTopic(id);

        return Ok();
    }
}