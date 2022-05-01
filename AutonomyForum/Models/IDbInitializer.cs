namespace AutonomyForum.Models;

public interface IDbInitializer : IDisposable
{
    Task InitializeAsync();
}