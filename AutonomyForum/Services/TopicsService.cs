using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class TopicsService
{
    private readonly TopicsRepository topicsRepository;

    public TopicsService(TopicsRepository topicsRepository)
        => this.topicsRepository = topicsRepository;

    public async Task<bool> TryCreateTopic(string title, string titleMessage, Guid authorId, Guid parentSectionId)
        => await topicsRepository.TryCreateTopic(title, titleMessage, authorId, parentSectionId);

    public async Task<TopicInfo?> FindTopic(Guid id)
    {
        var topic = await topicsRepository.FindTopic(id);

        if (topic == null)
        {
            return null;
        }

        return new TopicInfo(topic);
    }

    public async Task DeleteTopic(Guid id)
        => await topicsRepository.DeleteTopic(id);
}