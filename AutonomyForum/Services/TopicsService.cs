using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class TopicsService
{
    private readonly TopicsRepository topicsRepository;

    public TopicsService(TopicsRepository topicsRepository)
        => this.topicsRepository = topicsRepository;

    public async Task<bool> TryCreateTopicAsync(string title, string titleMessage, Guid authorId, Guid parentSectionId)
        => await topicsRepository.TryCreateTopicAsync(title, titleMessage, authorId, parentSectionId);

    public async Task<TopicInfo?> FindTopicAsync(Guid id)
    {
        var topic = await topicsRepository.FindTopicAsync(id);

        if (topic == null)
        {
            return null;
        }

        return new TopicInfo(topic);
    }

    public async Task DeleteTopicAsync(Guid id)
        => await topicsRepository.DeleteTopicAsync(id);
}