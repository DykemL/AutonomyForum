using System.ComponentModel.DataAnnotations;

namespace AutonomyForum.Models.DbEntities;

public abstract class DbEntity
{
    [Key]
    public Guid Id { get; }

    protected DbEntity()
        => Id = Guid.NewGuid();
}