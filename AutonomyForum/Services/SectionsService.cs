using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class SectionsService
{
    private readonly SectionsRepository sectionsRepository;

    public SectionsService(SectionsRepository sectionsRepository)
        => this.sectionsRepository = sectionsRepository;

    public async Task<SectionInfo[]> GetSections()
    {
        var sections = await sectionsRepository.GetSections();

        return sections.Select(x => new SectionInfo(x)).ToArray();
    }

    public async Task<SectionInfo?> FindSection(Guid id)
    {
        var section = await sectionsRepository.FindSection(id);
        if (section == null)
        {
            return null;
        }

        return new SectionInfo(section);
    }

    public async Task CreateSection(string title, string description)
        => await sectionsRepository.CreateSection(title, description);

    public async Task DeleteSection(Guid id)
        => await sectionsRepository.DeleteSection(id);
}