namespace AutonomyForum.Services.Claims.Permissions;

public static class Permissions
{
    #region Common
    public const string Ban = "ban";
    public const string SetModerator = "set-moderator";
    public const string SetPrefect = "set-prefect";
    public const string AllRestricted = "restricted";
    #endregion

    #region Section
    public const string CreateSection = "create-section";
    public const string ModifySection = "modify-section";
    public const string DeleteSection = "delete-section";
    #endregion

    #region Topic
    public const string DeleteTopic = "delete-topic";
    #endregion

    #region Reply
    public const string DeleteReply = "delete-reply";
    #endregion
}