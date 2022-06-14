namespace AutonomyForum.Models.DbEntities;

public class Election : DbEntity
{
    public Guid SectionId { get; set; }
    public Section Section { get; set; }

    private List<User>? candidates;
    public List<User> Candidates { get => candidates ??= new List<User>(); set => candidates = value; }

    private List<Vote>? votes;
    public List<Vote> Votes { get => votes ??= new List<Vote>(); set => votes = value; }

    public ElectionsStatus Status { get; set; } = ElectionsStatus.Registration;

    public DateTime? LastStatusModifiedDateTime { get; set; } = DateTime.UtcNow;

    public int CurrentRoundNumber { get; set; } = 1;
}