namespace AutonomyForum.Models.DbEntities;

public class TopicReply
{
    public User Author { get; set; }
    public DateTime CreationDateTime { get; set; }
    public string Message { get; set; }
}