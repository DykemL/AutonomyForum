using System.IdentityModel.Tokens.Jwt;
using AutonomyForum.Models.DbEntities;

namespace AutonomyForum.Services;

public interface IJwtSecurityService
{
    JwtSecurityToken CreateToken(User user, params string[] roles);
    string SerializeToken(JwtSecurityToken jwtSecurityToken);
}