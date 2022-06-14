using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class RepliesService
{
    private readonly RepliesRepository repliesRepository;
    private readonly UserService userService;

    public RepliesService(RepliesRepository repliesRepository, UserService userService)
    {
        this.repliesRepository = repliesRepository;
        this.userService = userService;
    }

    public async Task<bool> TryCreateReply(Guid parentTopicId, Guid authorId, string message)
    {
        var wasSuccess = await repliesRepository.TryCreateReply(parentTopicId, authorId, message);
        if (!wasSuccess)
        {
            return false;
        }

        var user = await userService.FindUserById(authorId);
        user!.RepliesCount++;
        await userService.UpdateUser(user);

        return true;
    }

    public async Task DeleteReply(Guid replyId)
        => await repliesRepository.DeleteReply(replyId);

    public async Task<bool> DoLikeReply(Guid replyId, Guid userId)
    {
        var wasSuccess = await repliesRepository.DoLikeReply(replyId, userId);
        if (!wasSuccess)
        {
            return false;
        }

        var reply = await repliesRepository.FindReply(replyId);
        var author = await userService.FindUserById(reply.AuthorId);
        author!.Rating++;
        await userService.UpdateUser(author);

        return true;
    }

    public async Task<bool> CancelLikeReply(Guid replyId, Guid userId)
    {
        var wasSuccess = await repliesRepository.CancelLikeReply(replyId, userId);
        if (!wasSuccess)
        {
            return false;
        }

        var reply = await repliesRepository.FindReply(replyId);
        var author = await userService.FindUserById(reply.AuthorId);
        author!.Rating--;
        await userService.UpdateUser(author);

        return true;
    }

    public async Task<bool> IsPrefect(Guid replyId, Guid userId)
    {
        var reply = await repliesRepository.FindReply(replyId);
        var topic = reply?.Topic;
        return topic?.Section.PrefectId == userId;
    }
}