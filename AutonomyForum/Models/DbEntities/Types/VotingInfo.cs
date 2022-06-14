namespace AutonomyForum.Models.DbEntities.Types;

public class VotingInfo
{
    public UserInfo Candidate { get; set; }
    public int Votes { get; set; }
}