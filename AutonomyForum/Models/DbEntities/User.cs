using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Models.DbEntities;

public sealed class User : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }

    public User(string userName) : base(userName)
    {
    }
}