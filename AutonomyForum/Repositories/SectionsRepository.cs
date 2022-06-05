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

    public async Task CreateSection(string title, string description, SectionType sectionType = SectionType.Main)
    {
        var section = new Section() { Title = title, Description = description, Type = sectionType };
        appDbContext.Sections.Add(section);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<Section[]> GetSections()
        => await appDbContext.Sections
                             .OrderByDescending(x => x.CreationDateTime)
                             .Include(x => x.Topics)
                             .ToArrayAsync();

    public async Task<Section?> FindSection(Guid id)
        => await appDbContext.Sections.Where(x => x.Id == id)
                             .Include(x => x.Topics!.OrderByDescending(y => y.CreationDateTime))!
                             .ThenInclude(x => x.Author)
                             .ThenInclude(x => x.AvatarFile)
                             .FirstOrDefaultAsync();

    public async Task DeleteSection(Guid id)
    {
        var section = new Section() { Id = id };
        appDbContext.Sections.Attach(section);
        appDbContext.Sections.Remove(section);
        await appDbContext.SaveChangesAsync();
    }

    public async Task UpdateSection(Section section)
    {
        appDbContext.Sections.Update(section);
        await appDbContext.SaveChangesAsync();
    }
}