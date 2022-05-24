using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using AutonomyForum.Services;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class TopicsRepository
{
    private readonly AppDbContext appDbContext;
    private readonly UserService userService;
    private readonly SectionsRepository sectionsRepository;

    public TopicsRepository(AppDbContext appDbContext, UserService userService, SectionsRepository sectionsRepository)
    {
        this.appDbContext = appDbContext;
        this.userService = userService;
        this.sectionsRepository = sectionsRepository;
    }

    public async Task<bool> TryCreateTopicAsync(string title, string titleMessage, Guid authorId, Guid parentSectionId)
    {
        var parentSection = await sectionsRepository.FindSectionAsync(parentSectionId);
        var user = await userService.FindUserByIdAsync(authorId);
        if (parentSection == null || user == null)
        {
            return false;
        }

        var topic = new Topic() { Title = title, TitleMessage = titleMessage, Author = user, Section = parentSection };
        appDbContext.Topics.Add(topic);
        await appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Topic?> FindTopicAsync(Guid id)
    {
        var topic = await appDbContext.Topics.Where(x => x.Id == id)
                                      .Include(x => x.Author)
                                      .Include(x => x.Replies)
                                      .ThenInclude(x => x.Author)
                                      .Include(x => x.Replies!.OrderBy(y => y.CreationDateTime))!
                                      .ThenInclude(x => x.FavoredBy)
                                      .FirstOrDefaultAsync();
        if (topic == null)
        {
            return null;
        }
        foreach (var reply in topic?.Replies!)
        {
            reply.Topic = null;
        }

        return topic;
    }

    public async Task DeleteTopicAsync(Guid id)
    {
        var topic = new Topic() { Id = id };
        appDbContext.Topics.Attach(topic);
        appDbContext.Topics.Remove(topic);
        await appDbContext.SaveChangesAsync();
    }
}