namespace AutonomyForum.Models.DbEntities.Types;

public class UserExtended
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string[] Roles { get; set; }
    public string[] Permissions { get; set; }
    public string? AvatarFilePath { get; set; }
}