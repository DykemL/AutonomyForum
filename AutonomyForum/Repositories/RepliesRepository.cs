using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class RepliesRepository
{
    private readonly AppDbContext appDbContext;
    private readonly DbSet<Reply> replies;

    private readonly UsersRepository usersRepository;

    public RepliesRepository(AppDbContext appDbContext, UsersRepository usersRepository)
    {
        this.appDbContext = appDbContext;
        this.usersRepository = usersRepository;
        replies = appDbContext.Set<Reply>();
    }

    public async Task<bool> TryCreateReply(Guid parentTopicId, Guid authorId, string message)
    {
        var reply = new Reply()
        {
            TopicId = parentTopicId,
            AuthorId = authorId,
            Message = message
        };
        replies.Add(reply);
        var created = await appDbContext.SaveChangesAsync();
        if (created == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<Reply?> FindReply(Guid replyId)
        => await replies.Include(x => x.FavoredBy)
                        .Include(x => x.Topic)
                        .ThenInclude(x => x.Section)
                        .FirstOrDefaultAsync(x => x.Id == replyId);

    public async Task DeleteReply(Guid replyId)
    {
        var reply = await appDbContext.Replies.FirstOrDefaultAsync(x => x.Id == replyId);
        appDbContext.Replies.Remove(reply!);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<bool> DoLikeReply(Guid replyId, Guid userId)
    {
        var user = await usersRepository.FindUserById(userId);
        var reply = await FindReply(replyId);
        if (user == null || reply == null || user.FavoredReplies.Any(x => x.Id == replyId) || reply.AuthorId == userId)
        {
            return false;
        }

        reply.FavoredBy.Add(user);
        appDbContext.Entry(reply).State = EntityState.Modified;
        await appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CancelLikeReply(Guid replyId, Guid userId)
    {
        var user = await usersRepository.FindUserById(userId);
        var reply = await FindReply(replyId);
        if (user == null || reply == null || reply.FavoredBy.FirstOrDefault(x => x.Id == userId) == null)
        {
            return false;
        }

        reply.FavoredBy.Remove(user);
        appDbContext.Entry(reply).State = EntityState.Modified;
        await appDbContext.SaveChangesAsync();

        return true;
    }
}