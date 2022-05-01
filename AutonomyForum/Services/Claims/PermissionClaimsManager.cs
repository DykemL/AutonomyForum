namespace AutonomyForum.Services.Claims;

public class PermissionClaimsManager
{
    public IReadOnlySet<string> Claims { get; }

    public PermissionClaimsManager()
        => Claims = GetAllClaims();

    private IReadOnlySet<string> GetAllClaims()
        => GetProperties(typeof(BaseClaims)).Concat(GetProperties(typeof(ConditionalClaims)))
                                            .ToHashSet();

    private string[] GetProperties(Type type)
        => type.GetProperties().Select(x => (string)x.GetRawConstantValue()!).ToArray();

    public static string GenerateBaseClaim(string claim)
        => $"b:{claim}";

    public static string GenerateSectionAccessClaim(Guid sectionId)
        => $"sa:{sectionId}";
}