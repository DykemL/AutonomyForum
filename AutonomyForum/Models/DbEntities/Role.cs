using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Models.DbEntities;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }
}