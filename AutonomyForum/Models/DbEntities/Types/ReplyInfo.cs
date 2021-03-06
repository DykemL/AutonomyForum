namespace AutonomyForum.Models.DbEntities.Types;

public class ReplyInfo
{
    public Guid Id { get; set; }
    public UserInfo? Author { get; set; }
    public TopicInfo? Topic { get; set; }
    public string Message { get; set; }
    public Guid[] FavoredBy { get; set; }
    public UserInfo[]? FavoredUsersBy { get; set; }
    public DateTime CreationDateTime { get; set; }

    public ReplyInfo(Reply reply)
    {
        Id = reply.Id;
        Author = reply.Author != null ? new UserInfo(reply.Author, reply.Author.AvatarFile?.Path) : null;
        Topic = reply.Topic != null ? new TopicInfo(reply.Topic) : null;
        Message = reply.Message;
        FavoredBy = reply.FavoredBy?.Select(x => x.Id).ToArray();
        FavoredUsersBy = reply.FavoredBy?.Select(x => new UserInfo(x, null)).ToArray();
        CreationDateTime = reply.CreationDateTime;
    }
}