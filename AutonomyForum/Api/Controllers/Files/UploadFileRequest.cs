using System.ComponentModel.DataAnnotations;

namespace AutonomyForum.Api.Controllers.Files;

public class UploadFileRequest
{
    [Required]
    public IFormFile File { get; set; }
}