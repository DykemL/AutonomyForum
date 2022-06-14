namespace AutonomyForum.Api.Controllers.Elections;

public class VoteRequest
{
    public Guid SectionId { get; set; }
    public Guid CandidateId { get; set; }
}