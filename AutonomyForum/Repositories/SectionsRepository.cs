using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using AutonomyForum.Models.DbEntities.Types;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class SectionsRepository
{
    private readonly AppDbContext appDbContext;

    public SectionsRepository(AppDbContext appDbContext)
        => this.appDbContext = appDbContext;

    public async Task CreateSectionAsync(string title, string description, SectionType sectionType = SectionType.Main)
    {
        var section = new Section() { Title = title, Description = description, Type = sectionType };
        appDbContext.Sections.Add(section);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<Section[]> GetSectionsAsync()
        => await appDbContext.Sections
                             .OrderByDescending(x => x.CreationDateTime)
                             .Include(x => x.Topics)
                             .ToArrayAsync();

    public async Task<Section?> FindSectionAsync(Guid id)
        => await appDbContext.Sections.Where(x => x.Id == id)
                             .Include(x => x.Topics!.OrderByDescending(y => y.CreationDateTime))!
                             .ThenInclude(x => x.Author)
                             .FirstOrDefaultAsync();

    public async Task DeleteSectionAsync(Guid id)
    {
        var section = new Section() { Id = id };
        appDbContext.Sections.Attach(section);
        appDbContext.Sections.Remove(section);
        await appDbContext.SaveChangesAsync();
    }

    public async Task UpdateSectionAsync(Section section)
    {
        appDbContext.Sections.Update(section);
        await appDbContext.SaveChangesAsync();
    }
}