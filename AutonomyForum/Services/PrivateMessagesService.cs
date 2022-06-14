using AutonomyForum.Models.DbEntities.Types;
using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class PrivateMessagesService
{
    private readonly PrivateMessagesRepository privateMessagesRepository;

    public PrivateMessagesService(PrivateMessagesRepository privateMessagesRepository)
        => this.privateMessagesRepository = privateMessagesRepository;

    public async Task<PrivateMessageInfo[]> GetPrivateMessages(Guid userId, Guid collocutorId)
    {
        var messagesByReceiver = await privateMessagesRepository.GetPrivateMessagesByReceiverId(userId);
        var messagesBySender = await privateMessagesRepository.GetPrivateMessagesBySenderId(userId);
        return messagesByReceiver.Where(x => x.SenderId == collocutorId)
                                 .Concat(messagesBySender.Where(x => x.ReceiverId == collocutorId))
                                 .OrderBy(x => x.CreationDateTime)
                                 .Select(x => new PrivateMessageInfo(x)).ToArray();
    }

    public async Task<UserInfo[]> GetCollocutors(Guid userId)
    {
        var messagesByReceiver = await privateMessagesRepository.GetPrivateMessagesByReceiverId(userId);
        var collocutorsBySender = messagesByReceiver.Select(x => new UserInfo(x.Sender, null))
                                                    .ToArray();

        var messagesBySender = await privateMessagesRepository.GetPrivateMessagesBySenderId(userId);
        var collocutorsByReceiver = messagesBySender.Select(x => new UserInfo(x.Receiver, null))
                                                    .ToArray();

        return collocutorsBySender.Concat(collocutorsByReceiver).OrderBy(x => x.UserName).DistinctBy(x => x.Id).ToArray();
    }

    public async Task AddMessage(Guid senderId, Guid receiverId, string message)
        => await privateMessagesRepository.AddMessage(senderId, receiverId, message);
}