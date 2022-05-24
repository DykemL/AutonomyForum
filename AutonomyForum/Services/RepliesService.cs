using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class RepliesService
{
    private readonly RepliesRepository repliesRepository;

    public RepliesService(RepliesRepository repliesRepository)
        => this.repliesRepository = repliesRepository;

    public async Task<bool> TryCreateReply(Guid parentTopicId, Guid authorId, string message)
        => await repliesRepository.TryCreateReplyAsync(parentTopicId, authorId, message);

    public async Task DeleteReplyAsync(Guid replyId)
        => await repliesRepository.DeleteReplyAsync(replyId);

    public async Task<bool> DoLikeReply(Guid replyId, Guid userId)
        => await repliesRepository.DoLikeReply(replyId, userId);
}