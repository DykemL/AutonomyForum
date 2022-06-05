using AutonomyForum.Models.DbEntities;
using Microsoft.AspNetCore.Identity;

namespace AutonomyForum.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserService userService;
    private readonly UserManager<User> userManager;
    private readonly IJwtSecurityService jwtSecurityService;

    public AuthService(UserService userService, UserManager<User> userManager, IJwtSecurityService jwtSecurityService)
    {
        this.userService = userService;
        this.userManager = userManager;
        this.jwtSecurityService = jwtSecurityService;
    }

    public async Task<AuthInfo?> Login(string userName, string password)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return null;
        }

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtSecurityService.CreateToken(user, roles.ToArray());
        user.RefreshToken = Guid.NewGuid().ToString();
        await userService.UpdateUser(user);
        var userExtended = await userService.GetUserExtended(user.Id);
        return new AuthInfo
        {
            Token = jwtSecurityService.SerializeToken(token),
            Expiration = token.ValidTo,
            RefreshToken = user.RefreshToken,
            UserExtended = userExtended!
        };
    }

    public async Task<RegisterStatus> Register(string userName, string email, string password, params string[] roles)
    {
        var userExists = await userManager.FindByNameAsync(userName);
        if (userExists != null)
        {
            return RegisterStatus.AlreadyExists;
        }

        var user = new User(userName) { Email = email };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return RegisterStatus.Error;
        }

        await userManager.AddToRolesAsync(user, roles);

        return RegisterStatus.Success;
    }

    public async Task<AuthInfo?> Refresh(string refreshToken)
    {
        var user = await userService.FindUserByRefreshToken(refreshToken);
        if (user == null)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtSecurityService.CreateToken(user, roles.ToArray());
        user.RefreshToken = Guid.NewGuid().ToString();
        await userService.UpdateUser(user);
        var userExtended = await userService.GetUserExtended(user.Id);
        return new AuthInfo
        {
            Token = jwtSecurityService.SerializeToken(token),
            Expiration = token.ValidTo,
            RefreshToken = user.RefreshToken,
            UserExtended = userExtended!
        };
    }
}