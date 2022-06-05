namespace AutonomyForum.Models.DbEntities.Types;

public class UserInfo
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? AvatarFilePath { get; set; }

    public UserInfo(User user, string avatarFilePath)
    {
        Id = user.Id;
        UserName = user.UserName;
        Email = user.Email;
        AvatarFilePath = avatarFilePath;
    }
}