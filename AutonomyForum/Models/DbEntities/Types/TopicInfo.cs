namespace AutonomyForum.Models.DbEntities.Types;

public class TopicInfo
{
    public Guid Id { get; set; }
    public UserInfo? Author { get; set; }
    public string Title { get; set; }
    public string TitleMessage { get; set; }
    public ReplyInfo[]? Replies { get; set; }
    public DateTime CreationDateTime { get; set; }

    public TopicInfo(Topic topic)
    {
        Id = topic.Id;
        Author = topic.Author != null ? new UserInfo(topic.Author, topic.Author.AvatarFile?.Path) : null;
        Title = topic.Title;
        TitleMessage = topic.TitleMessage;
        Replies = topic.Replies?.Select(x => new ReplyInfo(x)).ToArray();
        CreationDateTime = topic.CreationDateTime;
    }
}