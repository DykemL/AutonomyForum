using AutonomyForum.Models.DbEntities;
using AutonomyForum.Services;
using AutonomyForum.Services.Elections;

namespace AutonomyForum.HostedServices;

public class ElectionsHostedService : BackgroundService
{
    public readonly TimeSpan UpdateFrequency = TimeSpan.FromSeconds(6);

    private readonly IServiceScopeFactory serviceScopeFactory;
    private IServiceScope scope;
    private ElectionsService electionsService;
    private SectionsService sectionsService;

    public ElectionsHostedService(IServiceScopeFactory serviceScopeFactory)
        => this.serviceScopeFactory = serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken ctx)
    {
        while (!ctx.IsCancellationRequested)
        {
            UpdateScope();
            var elections = await electionsService.GetElections();
            await ProcessElections(elections);
            await Task.Delay(UpdateFrequency, ctx);
        }
    }

    public override Task StopAsync(CancellationToken ctx)
    {
        scope.Dispose();
        return Task.CompletedTask;
    }

    private void UpdateScope()
    {
        scope = serviceScopeFactory.CreateScope();
        electionsService = scope.ServiceProvider.GetService<ElectionsService>()!;
        sectionsService = scope.ServiceProvider.GetService<SectionsService>()!;
    }

    private async Task ProcessElections(Election[] elections)
    {
        foreach (var election in elections)
        {
            switch (election.Status)
            {
                case ElectionsStatus.Registration:
                    await HandleRegistrationStatus(election);
                    break;
                case ElectionsStatus.Elections:
                    await HandleElectionsStatus(election);
                    break;
                default: 
                    throw new ArgumentOutOfRangeException(nameof(election.Status), election.Status.ToString());
            }
        }
    }

    private async Task HandleRegistrationStatus(Election election)
    {
        if (election.LastStatusModifiedDateTime + ElectionsConditionsCurrent.ElectionsPeriod > DateTime.UtcNow)
        {
            return;
        }

        if (election.Candidates.Count == 0)
        {
            election.LastStatusModifiedDateTime = DateTime.UtcNow;
            await electionsService.UpdateElection(election);
            return;
        }

        if (election.Candidates.Count == 1)
        {
            await SetWinner(election, election.Candidates.First().Id);

            return;
        }

        election.Status = ElectionsStatus.Elections;
        election.LastStatusModifiedDateTime = DateTime.UtcNow;
        await electionsService.UpdateElection(election);
    }

    private async Task HandleElectionsStatus(Election election)
    {
        if (election.LastStatusModifiedDateTime + ElectionsConditionsCurrent.PeriodBetweenRounds > DateTime.UtcNow)
        {
            return;
        }

        var candidatesByVotes = await GetSortedCandidatesByVotes(election.SectionId);
        if (candidatesByVotes.Length == 2)
        {
            var first = candidatesByVotes[0];
            var second = candidatesByVotes[1];
            if (first.Votes == second.Votes)
            {
                await SetNextRound(election);
                return;
            }

            await SetWinner(election, candidatesByVotes.First().CandidateId);
            return;
        }

        var firstCandidateVotes = candidatesByVotes.First().Votes;
        var votesSum = candidatesByVotes.Sum(x => x.Votes);
        if ((double)firstCandidateVotes / votesSum > 0.5)
        {
            await SetWinner(election, candidatesByVotes.First().CandidateId);
            return;
        }

        var secondCandidateVotes = candidatesByVotes[1].Votes;
        var additionalCandidates = new List<Guid>() { candidatesByVotes[0].CandidateId, candidatesByVotes[1].CandidateId };
        for (var i = 2; i < candidatesByVotes.Length; i++)
        {
            if (candidatesByVotes[i].Votes < secondCandidateVotes)
            {
                break;
            }

            additionalCandidates.Add(candidatesByVotes[i].CandidateId);
        }

        election.Candidates = election.Candidates.Where(x => additionalCandidates.Contains(x.Id)).ToList();
        await SetNextRound(election);
    }

    private async Task SetWinner(Election election, Guid winnerId)
    {
        election.Status = ElectionsStatus.Registration;
        election.LastStatusModifiedDateTime = DateTime.UtcNow;
        election.Candidates.Clear();
        election.Votes.Clear();
        election.CurrentRoundNumber = 1;
        await electionsService.UpdateElection(election);
        await sectionsService.SetPrefect(election.SectionId, winnerId);
    }

    private async Task SetNextRound(Election election)
    {
        election.Votes.Clear();
        election.LastStatusModifiedDateTime = DateTime.UtcNow;
        election.CurrentRoundNumber++;
        await electionsService.UpdateElection(election);
    }

    private async Task<(Guid CandidateId, int Votes)[]> GetSortedCandidatesByVotes(Guid electionId)
    {
        var electionInfo = await electionsService.GetElectionInfo(electionId);
        return electionInfo.VotingInfos.OrderByDescending(x => x.Votes).Select(x => (x.Candidate.Id, x.Votes)).ToArray();
    }
}