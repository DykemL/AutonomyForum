using AutonomyForum.Models.DbEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = AutonomyForum.Models.DbEntities.File;

namespace AutonomyForum.Models;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Section> Sections { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Reply> Replies { get; set; }
    public DbSet<File> Files { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.RefreshToken).IsUnique();
        ResolveManyRelations(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void ResolveManyRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .HasMany(x => x.FavoredReplies)
                    .WithMany(x => x.FavoredBy);

        modelBuilder.Entity<Reply>().HasOne(x => x.Author)
                    .WithMany();
    }
}