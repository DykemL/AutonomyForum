using AutonomyForum.Api.Authorization;
using AutonomyForum.Api.Controllers.Sections;
using AutonomyForum.Services.Claims.Permissions;
using BankServer.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(HttpHeaders.JsonContentHeader)]
public class SectionsController : ControllerBase
{
    [Route("{id}")]
    [HttpDelete]
    [RequirePermission(Permissions.DeleteSection)]
    public async Task<ActionResult> DeleteSectionAsync([FromRoute] DeleteTopicRequest deleteTopicRequest)
    {
        return Ok();
    }
}