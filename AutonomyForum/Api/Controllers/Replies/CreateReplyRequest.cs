namespace AutonomyForum.Api.Controllers.Replies;

public class CreateReplyRequest
{
    public Guid TopicId { get; set; }
    public string Message { get; set; }
}