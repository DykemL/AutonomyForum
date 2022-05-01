using AutonomyForum.Models;

namespace AutonomyForum.HostedServices;

public class DbInitializerHostedService : IHostedService
{
    private readonly IDbInitializer dbInitializer;

    public DbInitializerHostedService(IDbInitializer dbInitializer)
        => this.dbInitializer = dbInitializer;

    public async Task StartAsync(CancellationToken cancellationToken)
        => await dbInitializer.InitializeAsync();

    public Task StopAsync(CancellationToken cancellationToken)
    {
        dbInitializer.Dispose();
        return Task.CompletedTask;
    }
}