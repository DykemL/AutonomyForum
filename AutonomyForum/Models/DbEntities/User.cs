using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Models.DbEntities;

public sealed class User : IdentityUser<Guid>
{
    public string? RefreshToken { get; private set; }

    public User(string userName) : base(userName)
    {
    }

    public void GenerateNewRefreshToken()
        => RefreshToken = Guid.NewGuid().ToString();
}