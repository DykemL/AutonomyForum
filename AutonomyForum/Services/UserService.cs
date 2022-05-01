using AutonomyForum.Models.DbEntities;
using AutonomyForum.Repositories;

namespace AutonomyForum.Services;

public class UserService
{
    private readonly UsersRepository usersRepository;

    public UserService(UsersRepository usersRepository)
        => this.usersRepository = usersRepository;

    public async Task UpdateUserAsync(User user)
        => await usersRepository.UpdateUserAsync(user);

    public async Task<User?> FindUserByRefreshToken(string refreshToken)
        => await usersRepository.FindUserByRefreshToken(refreshToken);
}