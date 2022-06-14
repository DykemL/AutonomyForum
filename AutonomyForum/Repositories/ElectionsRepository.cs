using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class ElectionsRepository
{
    private readonly AppDbContext appDbContext;

    public ElectionsRepository(AppDbContext appDbContext)
        => this.appDbContext = appDbContext;

    public async Task<Election?> GetElection(Guid sectionId)
        => await appDbContext.Elections.Where(x => x.SectionId == sectionId)
                             .Include(x => x.Section)
                             .ThenInclude(x => x.Prefect)
                             .Include(x => x.Candidates)
                             .Include(x => x.Votes)
                             .FirstOrDefaultAsync();

    public async Task<Election[]> GetElections()
        => await appDbContext.Elections.OrderByDescending(x => x.Section.CreationDateTime)
                             .Include(x => x.Section)
                             .ThenInclude(x => x.Prefect)
                             .Include(x => x.Candidates)
                             .Include(x => x.Votes)
                             .ToArrayAsync();

    public async Task UpdateElection(Election election)
    {
        appDbContext.Elections.Update(election);
        await appDbContext.SaveChangesAsync();
    }
}