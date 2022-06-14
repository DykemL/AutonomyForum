namespace AutonomyForum.Models.DbEntities.Types;

public class ElectionInfo
{
    public Guid Id { get; set; }    

    public SectionInfo SectionInfo { get; set; }

    public List<UserInfo> Candidates { get; set; } = new();

    public List<VotingInfo> VotingInfos { get; set; } = new();

    public List<Guid> AlreadyVotedUserIds { get; set; } = new();

    public ElectionsStatus Status { get; set; }

    public DateTime? LastStatusModifiedDateTime { get; set; }

    public int CurrentRoundNumber { get; set; }

    public ElectionInfo(Election election)
    {
        Id = election.Id;
        SectionInfo = new SectionInfo(election.Section);
        Candidates = election.Candidates.OrderBy(x => x.UserName).Select(x => new UserInfo(x, null)).ToList();
        var votesSet = new Dictionary<Guid, int>();
        foreach (var candidate in election.Candidates)
        {
            votesSet.Add(candidate.Id, 0);
        }
        foreach (var vote in election.Votes)
        {
            if (votesSet.ContainsKey(vote.CandidateId))
            {
                AlreadyVotedUserIds.Add(vote.ElectorId);
                votesSet[vote.CandidateId]++;
            }
        }

        foreach (var vote in votesSet)
        {
            var candidate = new UserInfo(election.Candidates.First(x => x.Id == vote.Key), null);
            VotingInfos.Add(new VotingInfo()
            {
                Candidate = candidate,
                Votes = vote.Value
            });
        }

        Status = election.Status;
        LastStatusModifiedDateTime = election.LastStatusModifiedDateTime;
        CurrentRoundNumber = election.CurrentRoundNumber;
    }
}