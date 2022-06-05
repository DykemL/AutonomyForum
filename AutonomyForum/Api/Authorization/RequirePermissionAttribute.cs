using AutonomyForum.Extentions;
using AutonomyForum.Services;
using AutonomyForum.Services.Claims.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ClaimTypes = AutonomyForum.Services.Claims.ClaimTypes;

namespace AutonomyForum.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string permission;

    public RequirePermissionAttribute(string permission)
        => this.permission = permission;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.User.Identity!.Name;
        if (userName == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userService = context.HttpContext.RequestServices.GetService<UserService>()!;

        var userClaims = await userService.GetPermissionClaims(userName!);
        var permissionClaims = userClaims.Where(x => x.Type == ClaimTypes.Permission)
                                         .Select(x => x.Value)
                                         .ToHashSet();
        var basePermissions = permissionClaims.Where(x => x.StartsWith(PermissionPrefixes.Base + ":")).ToArray();
        var hasPermission = basePermissions.Contains(PermissionsGenerator.GenerateBase(permission));
        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}