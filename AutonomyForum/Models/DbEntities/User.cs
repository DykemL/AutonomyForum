using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Models.DbEntities;

public sealed class User : IdentityUser<Guid>
{
    public List<Reply> FavoredReplies { get; set; }

    public string? RefreshToken { get; set; }

    public int RepliesCount { get; set; } = 0;
    public int Rating { get; set; } = 0;

    public Guid? ElectionId { get; set; }
    public Election? Election { get; set; }

    public Guid? AvatarFileId { get; set; }
    public File? AvatarFile { get; set; }

    public User(string userName) : base(userName)
    {
    }
}