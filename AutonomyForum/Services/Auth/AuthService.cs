﻿using AutonomyForum.Api.Types.Requests;
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

    public async Task<AuthInfo?> AuthAsync(AuthRequest authRequest)
    {
        var user = await userManager.FindByNameAsync(authRequest.UserName);
        if (user == null)
        {
            return null;
        }

        if (!await userManager.CheckPasswordAsync(user, authRequest.Password))
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtSecurityService.CreateToken(user, roles.ToArray());
        user.GenerateNewRefreshToken();
        await userService.UpdateUserAsync(user);
        return new AuthInfo
        {
            Token = jwtSecurityService.SerializeToken(token),
            RefreshToken = user.RefreshToken!,
            Expiration = token.ValidTo
        };
    }

    public async Task<RegisterStatus> RegisterAsync(RegisterRequest registerRequest, params string[] roles)
    {
        var userExists = await userManager.FindByNameAsync(registerRequest.UserName);
        if (userExists != null)
        {
            return RegisterStatus.AlreadyExists;
        }

        var user = new User(registerRequest.UserName) { Email = registerRequest.Email };
        var result = await userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
        {
            return RegisterStatus.Error;
        }

        await userManager.AddToRolesAsync(user, roles);

        return RegisterStatus.Success;
    }

    public async Task<AuthInfo?> RefreshAsync(string refreshToken)
    {
        var user = await userService.FindUserByRefreshToken(refreshToken);
        if (user == null)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtSecurityService.CreateToken(user, roles.ToArray());
        user.GenerateNewRefreshToken();
        return new AuthInfo
        {
            Token = jwtSecurityService.SerializeToken(token),
            RefreshToken = user.RefreshToken!,
            Expiration = token.ValidTo
        };
    }
}