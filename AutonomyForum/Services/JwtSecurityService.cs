using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutonomyForum.Models.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AutonomyForum.Services;

public class JwtSecurityService : IJwtSecurityService
{
    private readonly TimeSpan jwtTtl = TimeSpan.FromMinutes(60); // Поменять на меньшее

    private readonly UserManager<User> userManager;
    private readonly AppSettings appSettings;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

    public JwtSecurityService(UserManager<User> userManager, AppSettings appSettings, JwtSecurityTokenHandler jwtSecurityTokenHandler)
    {
        this.userManager = userManager;
        this.appSettings = appSettings;
        this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public JwtSecurityToken CreateToken(User user, string[] roles)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSigningKey = new SymmetricSecurityKey(appSettings.JwtSigningSecretKey);

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(jwtTtl),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public string SerializeToken(JwtSecurityToken jwtSecurityToken)
        => jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
}