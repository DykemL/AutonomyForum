namespace AutonomyForum.Api.Controllers.PrivateMessages;

public class AddPrivateMessageRequest
{
    public Guid ReceiverId { get; set; }
    public string Message { get; set; }
}