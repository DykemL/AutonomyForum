using BankServer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutonomyForum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(HttpHeaders.JsonContentHeader)]
public class ForumController : ControllerBase
{
    [Route("test")]
    [Authorize]
    [HttpPost]
    public IActionResult Test()
    {
        return Ok();
    }
}