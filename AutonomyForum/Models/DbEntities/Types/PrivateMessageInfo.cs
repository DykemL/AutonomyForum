namespace AutonomyForum.Models.DbEntities.Types;

public class PrivateMessageInfo
{
    public UserInfo Sender { get; set; }

    public UserInfo Receiver { get; set; }

    public string Message { get; set; }

    public DateTime CreationDateTime { get; set; }

    public PrivateMessageInfo(PrivateMessage privateMessage)
    {
        Sender = new UserInfo(privateMessage.Sender, null);
        Receiver = new UserInfo(privateMessage.Sender, null);
        Message = privateMessage.Message;
        CreationDateTime = privateMessage.CreationDateTime;
    }
}