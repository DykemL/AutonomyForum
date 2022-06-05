using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using Microsoft.EntityFrameworkCore;
using File = AutonomyForum.Models.DbEntities.File;

namespace AutonomyForum.Repositories;

public class FilesRepository
{
    private readonly AppDbContext appDbContext;

    public FilesRepository(AppDbContext appDbContext)
        => this.appDbContext = appDbContext;

    public async Task AddFile(File file)
    {
        appDbContext.Files.Add(file);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<File?> GetFile(Guid fileId)
        => await appDbContext.Files.FirstOrDefaultAsync(x => x.Id == fileId);
}