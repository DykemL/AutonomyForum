using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using AutonomyForum.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class UsersRepository
{
    private readonly AppDbContext appDbContext;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly FilesService filesService;

    public UsersRepository(AppDbContext appDbContext, UserManager<User> userManager, RoleManager<Role> roleManager, FilesService filesService)
    {
        this.appDbContext = appDbContext;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.filesService = filesService;
    }

    public async Task UpdateUser(User user)
    {
        appDbContext.Users.Update(user);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<User?> FindUserById(Guid id)
        => await appDbContext.Users.Where(x => x.Id == id)
                             .Include(x => x.FavoredReplies)
                             .Include(x => x.AvatarFile)
                             .FirstOrDefaultAsync();

    public async Task<User?> FindUserByRefreshToken(string refreshToken)
        => await appDbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

    public async Task<bool> TryAttachAvatarToUser(Guid userId, Guid fileId)
    {
        var file = await filesService.GetFile(fileId);
        var user = await FindUserById(userId);
        if (file == null || user == null)
        {
            return false;
        }
        user.AvatarFile = file;
        await UpdateUser(user);

        return true;
    }

    public async Task<bool> AddRoleToUser(Guid userId, string userRole)
    {
        if (!await roleManager.RoleExistsAsync(userRole))
        {
            return false;
        }
        var user = await FindUserById(userId);
        if (user == null)
        {
            return false;
        }

        await userManager.AddToRoleAsync(user, userRole);
        return true;
    }

    public async Task<bool> RemoveRoleFromUser(Guid userId, string userRole)
    {
        if (!await roleManager.RoleExistsAsync(userRole))
        {
            return false;
        }
        var user = await FindUserById(userId);
        if (user == null)
        {
            return false;
        }

        await userManager.RemoveFromRoleAsync(user, userRole);
        return true;
    }
}