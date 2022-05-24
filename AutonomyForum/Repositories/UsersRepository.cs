using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class UsersRepository
{
    private readonly AppDbContext appDbContext;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;

    public UsersRepository(AppDbContext appDbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        this.appDbContext = appDbContext;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task UpdateUserAsync(User user)
    {
        appDbContext.Users.Update(user);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<User?> FindUserByIdAsync(Guid id)
        => await appDbContext.Users.Where(x => x.Id == id)
                             .Include(x => x.FavoredReplies)
                             .FirstOrDefaultAsync();

    public async Task<User?> FindUserByRefreshTokenAsync(string refreshToken)
        => await appDbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

    public async Task<bool> AddRoleToUser(Guid userId, string userRole)
    {
        if (!await roleManager.RoleExistsAsync(userRole))
        {
            return false;
        }
        var user = await FindUserByIdAsync(userId);
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
        var user = await FindUserByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        await userManager.RemoveFromRoleAsync(user, userRole);
        return true;
    }
}