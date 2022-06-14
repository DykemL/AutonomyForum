namespace AutonomyForum.Models.DbEntities.Types;

public class ElectionsSummaryInfo
{
    public TimeSpan VotingsPeriod { get; set; }
    public DateTime LastVotingDateTime { get; set; }
    public int RepliesNeededToVote { get; set; }
    public int RatingNeededToVote { get; set; }
    public int RepliesNeededToBeElected { get; set; }
    public int RatingNeededToBeElected { get; set; }
}