using AutonomyForum.Models.DbEntities;
using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;

namespace AutonomyForum.Services.Elections;

public class ElectionsService
{
    private readonly ElectionsRepository electionsRepository;
    private readonly VotesRepository votesRepository;
    private readonly UserService usersService;

    public ElectionsService(ElectionsRepository electionsRepository,
                            VotesRepository votesRepository,
                            UserService usersService)
    {
        this.electionsRepository = electionsRepository;
        this.votesRepository = votesRepository;
        this.usersService = usersService;
    }

    public async Task<bool> TryRegisterForElections(Guid sectionId, Guid userId)
    {
        var candidate = await usersService.FindUserById(userId);
        if (!IsUserMatchingToBeElected(candidate))
        {
            return false;
        }
        var election = await electionsRepository.GetElection(sectionId);
        if (election.Candidates.Any(x => x.Id == userId))
        {
            return false;
        }

        election.Candidates.Add(candidate);
        await electionsRepository.UpdateElection(election);

        return true;
    }

    public async Task<bool> TryRevokeRegisterForElections(Guid sectionId, Guid userId)
    {
        var election = await electionsRepository.GetElection(sectionId);
        var candidate = election?.Candidates.Find(x => x.Id == userId);
        if (candidate == null)
        {
            return false;
        }

        election!.Candidates.Remove(candidate);
        await electionsRepository.UpdateElection(election);

        return true;
    }

    public async Task<bool> TryVote(Guid sectionId, Guid electorId, Guid candidateId)
    {
        if (electorId == candidateId)
        {
            return false;
        }
        var elector = await usersService.FindUserById(electorId);
        if (!IsUserMatchingForVoting(elector))
        {
            return false;
        }
        var election = await electionsRepository.GetElection(sectionId);
        if (!election.Candidates.Any(x => x.Id == candidateId))
        {
            return false;
        }
        if (election.Votes.Any(x => x.ElectionId == election.Id && x.ElectorId == electorId))
        {
            return false;
        }

        await votesRepository.AddVote(election.Id, candidateId, electorId);
        return true;
    }

    public async Task<ElectionInfo> GetElectionInfo(Guid sectionId)
    {
        var election = await electionsRepository.GetElection(sectionId);
        return new ElectionInfo(election);
    }

    public async Task<ElectionInfo[]> GetElectionInfos()
    {
        var elections = await electionsRepository.GetElections();
        var electionInfos = new List<ElectionInfo>();
        foreach (var election in elections)
        {
            electionInfos.Add(new ElectionInfo(election));
        }

        return electionInfos.ToArray();
    }

    public ElectionsConditions GetElectionsConditions()
        => new();

    public async Task<Election[]> GetElections()
        => await electionsRepository.GetElections();

    public async Task UpdateElection(Election election)
        => await electionsRepository.UpdateElection(election);

    private bool IsUserMatchingToBeElected(User user)
        => user.Rating >= ElectionsConditionsCurrent.RatingNeededToBeElected &&
           user.RepliesCount >= ElectionsConditionsCurrent.RepliesNeededToBeElected;

    private bool IsUserMatchingForVoting(User user)
        => user.Rating >= ElectionsConditionsCurrent.RatingNeededToVote &&
           user.RepliesCount >= ElectionsConditionsCurrent.RepliesNeededToVote;
}