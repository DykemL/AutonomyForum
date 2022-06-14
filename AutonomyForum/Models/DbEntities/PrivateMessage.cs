namespace AutonomyForum.Models.DbEntities;

public class PrivateMessage : DbEntity
{
    public Guid SenderId { get; set; }
    public User Sender { get; set; }

    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; }

    public string Message { get; set; }

    public DateTime CreationDateTime { get; set; }

    public PrivateMessage()
        => CreationDateTime = DateTime.UtcNow;
}