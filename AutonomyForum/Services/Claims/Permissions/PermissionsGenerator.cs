using System.Security.Claims;

namespace AutonomyForum.Services.Claims.Permissions;

public static class PermissionsGenerator
{
    public static string GenerateBase(string permission)
        => $"{PermissionPrefixes.Base}:{permission}";

    public static string GenerateConditional(string permission)
        => $"{PermissionPrefixes.Conditional}:{permission}";

    public static Claim GenerateBaseClaim(string permission)
        => new (ClaimTypes.Permission, GenerateBase(permission));

    public static Claim GenerateConditionalClaim(string permission)
        => new (ClaimTypes.Permission, GenerateConditional(permission));

    public static string GenerateSectionAccessClaim(Guid sectionId)
        => $"{PermissionPrefixes.ConditionalApprove}:{PermissionPrefixes.Section}:{sectionId}";
}