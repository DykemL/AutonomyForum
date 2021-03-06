using AutonomyForum.Models.DbEntities.Types;

namespace AutonomyForum.Models.DbEntities;

public class Section : DbEntity
{
    public SectionType Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Topic> Topics { get; set; }

    public Guid? PrefectId { get; set; }
    public User? Prefect { get; set; }

    public Guid ElectionId { get; set; }
    public Election Election { get; set; }

    public DateTime CreationDateTime { get; set; }

    public Section()
        => CreationDateTime = DateTime.UtcNow;
}