namespace AutonomyForum.Models.DbEntities.Types;

public class SectionInfo
{
    public Guid Id { get; set; }
    public SectionType Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TopicInfo[]? Topics { get; set; }

    public SectionInfo(Section section)
    {
        Id = section.Id;
        Type = section.Type;
        Title = section.Title;
        Description = section.Description;
        Topics = section.Topics?.Select(x => new TopicInfo(x)).ToArray();
    }
}