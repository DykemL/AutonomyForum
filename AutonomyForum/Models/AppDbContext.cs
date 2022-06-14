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

    public DbSet<Election> Elections { get; set; }
    public DbSet<Vote> Votes { get; set; }

    public DbSet<PrivateMessage> PrivateMessages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.RefreshToken).IsUnique();
        ResolveManyRelations(modelBuilder);
        ResolveOneToOneRelations(modelBuilder);

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

    private void ResolveOneToOneRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Section>().HasOne(x => x.Election)
                    .WithOne(x => x.Section)
                    .HasForeignKey<Election>(x => x.SectionId);

        modelBuilder.Entity<Election>().HasOne(x => x.Section)
                    .WithOne(x => x.Election)
                    .HasForeignKey<Section>(x => x.ElectionId);
    }
}