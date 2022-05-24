namespace AutonomyForum.Models.DbEntities;

public class Topic : DbEntity
{
    public Guid SectionId { get; set; }
    public Section Section { get; set; }

    public Guid AuthorId { get; set; }
    public User Author { get; set; }

    public string Title { get; set; }

    public string TitleMessage { get; set; }

    public List<Reply> Replies { get; set; }

    public DateTime CreationDateTime { get; set; }

    public Topic()
        => CreationDateTime = DateTime.UtcNow;
}