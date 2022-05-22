namespace AutonomyForum.Models.DbEntities;

public class Topic : DbEntity
{
    public User Author { get; set; }
    public string Description { get; set; }
    public DateTime CreationDateTime { get; set; }
    public TopicReply[] Replies { get; set; }
}