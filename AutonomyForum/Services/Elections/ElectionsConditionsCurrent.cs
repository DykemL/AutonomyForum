namespace AutonomyForum.Services.Elections;

public static class ElectionsConditionsCurrent
{
    public const int RepliesNeededToVote = 30;
    public const int RatingNeededToVote = 10;

    public const int RepliesNeededToBeElected = 100;
    public const int RatingNeededToBeElected = 40;

    public static readonly TimeSpan ElectionsPeriod = TimeSpan.FromMinutes(1);
    public static readonly TimeSpan PeriodBetweenRounds = TimeSpan.FromMinutes(1);
}