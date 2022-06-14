using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;

namespace AutonomyForum.Repositories;

public class VotesRepository
{
    private readonly AppDbContext appDbContext;

    public VotesRepository(AppDbContext appDbContext)
        => this.appDbContext = appDbContext;

    public async Task AddVote(Guid electionId, Guid candidateId, Guid electorId)
    {
        var vote = new Vote()
        {
            ElectionId = electionId,
            CandidateId = candidateId,
            ElectorId = electorId
        };
        appDbContext.Votes.Add(vote);
        await appDbContext.SaveChangesAsync();
    }
}