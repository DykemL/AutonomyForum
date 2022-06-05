using AutonomyForum.Api.Authorization;
using AutonomyForum.Api.Controllers.Sections;
using AutonomyForum.Helpers;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services;
using AutonomyForum.Services.Claims.Permissions;
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
    public async Task<ActionResult> CreateSection([FromBody] CreateSectionRequest request)
    {
        await sectionsService.CreateSection(request.Title, request.Description);
        return Ok();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<SectionInfo[]>> GetSections()
    {
        var sectionInfos = await sectionsService.GetSections();
        return Ok(sectionInfos);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public async Task<ActionResult<SectionInfo>> GetSectionById([FromRoute] GetSectionByIdRequest request)
    {
        var section = await sectionsService.FindSection(request.Id);
        return Ok(section);
    }

    [HttpDelete]
    [RequirePermission(Permissions.DeleteSection)]
    [Route("{id}")]
    public async Task<ActionResult> DeleteSection([FromRoute] DeleteSectionRequest request)
    {
        await sectionsService.DeleteSection(request.Id);
        return Ok();
    }
}