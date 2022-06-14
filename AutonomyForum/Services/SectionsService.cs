using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;
using AutonomyForum.Services.Roles;

namespace AutonomyForum.Services;

public class SectionsService
{
    private readonly SectionsRepository sectionsRepository;
    private readonly UsersRepository usersRepository;

    public SectionsService(SectionsRepository sectionsRepository, UsersRepository usersRepository)
    {
        this.sectionsRepository = sectionsRepository;
        this.usersRepository = usersRepository;
    }

    public async Task<SectionInfo[]> GetSections()
    {
        var sections = await sectionsRepository.GetMainSections();

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
        => await sectionsRepository.CreateMainSection(title, description);

    public async Task DeleteSection(Guid id)
        => await sectionsRepository.DeleteSection(id);

    public async Task SetPrefect(Guid sectionId, Guid userId)
    {
        var section = await sectionsRepository.FindSection(sectionId);
        var oldPrefectId = section?.Prefect?.Id;
        if (oldPrefectId.HasValue)
        {
            await usersRepository.RemoveRoleFromUser(oldPrefectId.Value, AppRoles.Prefect);
        }
        var user = await usersRepository.FindUserById(userId);
        await usersRepository.AddRoleToUser(userId, AppRoles.Prefect);
        section.Prefect = user;
        await sectionsRepository.UpdateSection(section);
    }
}