namespace AutonomyForum.Services.Elections;

public class ElectionsConditions
{
    public int RepliesNeededToVote { get; set; }
    public int RatingNeededToVote { get; set; }

    public int RepliesNeededToBeElected { get; set; }
    public int RatingNeededToBeElected { get; set; }

    public TimeSpan ElectionsPeriod { get; set; }
    public TimeSpan PeriodBetweenRounds { get; set; }

    public ElectionsConditions()
    {
        RepliesNeededToVote = ElectionsConditionsCurrent.RepliesNeededToVote;
        RatingNeededToVote = ElectionsConditionsCurrent.RatingNeededToVote;
        RepliesNeededToBeElected = ElectionsConditionsCurrent.RepliesNeededToBeElected;
        RatingNeededToBeElected = ElectionsConditionsCurrent.RatingNeededToBeElected;
        ElectionsPeriod = ElectionsConditionsCurrent.ElectionsPeriod;
        PeriodBetweenRounds = ElectionsConditionsCurrent.PeriodBetweenRounds;
    }
}