using AutonomyForum.Api.Controllers.Elections;
using AutonomyForum.Extentions;
using AutonomyForum.Helpers;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Services.Elections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Produces(HttpHeaders.JsonContentHeader)]
[Authorize]
[Route("api/elections")]
public class ElectionsController : ControllerBase
{
    private readonly ElectionsService electionsService;

    public ElectionsController(ElectionsService electionsService)
        => this.electionsService = electionsService;

    [HttpPost]
    [Route("{sectionId}/register")]
    public async Task<ActionResult> Register([FromRoute] Guid sectionId)
    {
        if (!await electionsService.TryRegisterForElections(sectionId, User.GetId()))
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [Route("{sectionId}/revoke-register")]
    public async Task<ActionResult> RevokeRegister([FromRoute] Guid sectionId)
    {
        if (!await electionsService.TryRevokeRegisterForElections(sectionId, User.GetId()))
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [Route("{sectionId}/vote/{candidateId}")]
    public async Task<ActionResult> Vote([FromRoute] VoteRequest request)
    {
        if (!await electionsService.TryVote(request.SectionId, User.GetId(), request.CandidateId))
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{sectionId}")]
    public async Task<ActionResult<ElectionInfo>> GetInfo([FromRoute] Guid sectionId)
    {
        var electionInfo = await electionsService.GetElectionInfo(sectionId);

        return Ok(electionInfo);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ElectionInfo[]>> GetInfos()
    {
        var electionInfos = await electionsService.GetElectionInfos();

        return Ok(electionInfos);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("conditions")]
    public ActionResult<ElectionsConditions> GetElectionsConditions()
    {
        var electionsConditions = electionsService.GetElectionsConditions();

        return Ok(electionsConditions);
    }
}