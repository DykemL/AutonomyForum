namespace AutonomyForum.Models.DbEntities;

public class Vote : DbEntity
{
    public Guid ElectionId { get; set; }
    public Election Election { get; set; }

    public Guid CandidateId { get; set; }
    public User Candidate { get; set; }

    public Guid ElectorId { get; set; }
    public User Elector { get; set; }
}