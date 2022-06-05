using System.ComponentModel.DataAnnotations;

namespace AutonomyForum.Models.DbEntities;

public class DbEntity
{
    [Key]
    public Guid Id { get; set; }

    public DbEntity()
        => Id = Guid.NewGuid();
}