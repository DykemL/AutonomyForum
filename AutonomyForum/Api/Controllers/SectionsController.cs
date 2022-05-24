using AutonomyForum.Api.Authorization;
using AutonomyForum.Api.Controllers.Sections;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using AutonomyForum.Services.Claims.Permissions;
using BankServer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/sections")]
[Produces(HttpHeaders.JsonContentHeader)]
[Authorize]
public class SectionsController : ControllerBase
{
    private readonly SectionsService sectionsService;

    public SectionsController(SectionsService sectionsService)
        => this.sectionsService = sectionsService;

    [HttpPut]
    [RequirePermission(Permissions.CreateSection)]
    public async Task<ActionResult> CreateSectionAsync([FromBody] CreateSectionRequest request)
    {
        await sectionsService.CreateSectionAsync(request.Title, request.Description);
        return Ok();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<SectionInfo[]>> GetSectionsAsync()
    {
        var sectionInfos = await sectionsService.GetSectionsAsync();
        return Ok(sectionInfos);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public async Task<ActionResult<SectionInfo>> GetSectionByIdAsync([FromRoute] GetSectionByIdRequest request)
    {
        var section = await sectionsService.FindSectionAsync(request.Id);
        return Ok(section);
    }

    [HttpDelete]
    [RequirePermission(Permissions.DeleteSection)]
    [Route("{id}")]
    public async Task<ActionResult> DeleteSectionAsync([FromRoute] DeleteSectionRequest request)
    {
        await sectionsService.DeleteSectionAsync(request.Id);
        return Ok();
    }
}