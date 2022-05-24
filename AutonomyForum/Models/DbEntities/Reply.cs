namespace AutonomyForum.Models.DbEntities;

public class Reply : DbEntity
{
    public Guid AuthorId { get; set; }
    public User Author { get; set; }

    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }

    public string Message { get; set; }

    public List<User> FavoredBy { get; set; }

    public DateTime CreationDateTime { get; set; }

    public Reply()
        => CreationDateTime = DateTime.UtcNow;
}