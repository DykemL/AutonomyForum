using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class SectionsService
{
    private readonly SectionsRepository sectionsRepository;

    public SectionsService(SectionsRepository sectionsRepository)
        => this.sectionsRepository = sectionsRepository;

    public async Task<SectionInfo[]> GetSectionsAsync()
    {
        var sections = await sectionsRepository.GetSectionsAsync();

        return sections.Select(x => new SectionInfo(x)).ToArray();
    }

    public async Task<SectionInfo?> FindSectionAsync(Guid id)
    {
        var section = await sectionsRepository.FindSectionAsync(id);
        if (section == null)
        {
            return null;
        }

        return new SectionInfo(section);
    }

    public async Task CreateSectionAsync(string title, string description)
        => await sectionsRepository.CreateSectionAsync(title, description);

    public async Task DeleteSectionAsync(Guid id)
        => await sectionsRepository.DeleteSectionAsync(id);
}