namespace AutonomyForum.Models.DbEntities.Types;

public class UserInfo
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public UserInfo(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Email = user.Email;
    }
}