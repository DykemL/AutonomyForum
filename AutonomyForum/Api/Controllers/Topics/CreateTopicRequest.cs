namespace AutonomyForum.Api.Controllers.Topics;

public class CreateTopicRequest
{
    public string Title { get; set; }
    public string TitleMessage { get; set; }
    public Guid SectionId { get; set; }
}