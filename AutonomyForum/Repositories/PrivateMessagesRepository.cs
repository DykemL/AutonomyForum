using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class PrivateMessagesRepository
{
    private readonly AppDbContext appDbContext;

    public PrivateMessagesRepository(AppDbContext appDbContext)
        => this.appDbContext = appDbContext;

    public async Task<PrivateMessage[]> GetPrivateMessagesByReceiverId(Guid receiverId)
        => await appDbContext.PrivateMessages.Where(x => x.ReceiverId == receiverId)
                             .Include(x => x.Sender)
                             .Include(x => x.Receiver)
                             .ToArrayAsync();

    public async Task<PrivateMessage[]> GetPrivateMessagesBySenderId(Guid senderId)
        => await appDbContext.PrivateMessages.Where(x => x.SenderId == senderId)
                             .Include(x => x.Sender)
                             .Include(x => x.Receiver)
                             .ToArrayAsync();

    public async Task AddMessage(Guid senderId, Guid receiverId, string message)
    {
        var newMessage = new PrivateMessage()
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message
        };
        await appDbContext.PrivateMessages.AddAsync(newMessage);
        await appDbContext.SaveChangesAsync();
    }
}