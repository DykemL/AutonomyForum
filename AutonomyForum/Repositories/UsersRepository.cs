using AutonomyForum.Models;
using AutonomyForum.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace AutonomyForum.Repositories;

public class UsersRepository
{
    private readonly AppDbContext appDbContext;

    public UsersRepository(AppDbContext appDbContext)
        => this.appDbContext = appDbContext;

    public async Task UpdateUserAsync(User user)
    {
        appDbContext.Users.Update(user);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<User?> FindUserByRefreshToken(string refreshToken)
        => await appDbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
}