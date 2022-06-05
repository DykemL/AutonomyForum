using AutonomyForum.Api.Controllers.Files;
using AutonomyForum.Helpers;
using AutonomyForum.Services;
using Microsoft.AspNetCore.Mvc;
using File = AutonomyForum.Models.DbEntities.File;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/files")]
[Produces(HttpHeaders.JsonContentHeader)]
public class FilesController : ControllerBase
{
    private readonly FilesService filesService;
    private readonly IWebHostEnvironment appEnvironment;

    public FilesController(FilesService filesService, IWebHostEnvironment appEnvironment)
    {
        this.filesService = filesService;
        this.appEnvironment = appEnvironment;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<ActionResult<File>> UploadFile([FromForm] UploadFileRequest request)
    {
        var file = await filesService.UploadFile(request.File, appEnvironment.WebRootPath);
        return Ok(file);
    }

    [HttpGet]
    [Route("{fileId}")]
    public async Task<ActionResult<File>> GetFile([FromRoute] Guid fileId)
    {
        var file = await filesService.GetFile(fileId);
        return Ok(file);
    }
}