using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutonomyForum.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireClaimAttribute : Attribute, IAuthorizationFilter
{
    private readonly Claim claim;

    public RequireClaimAttribute(Claim claim)
        => this.claim = claim;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value);
        if (!hasClaim)
        {
            context.Result = new ForbidResult();
        }
    }
}