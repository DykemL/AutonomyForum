namespace AutonomyForum.Services.Claims.Permissions;

public static class Permissions
{
    #region Common
    public const string Ban = "ban";
    #endregion

    #region Section
    public const string CreateSection = "create-section";
    public const string ModifySection = "modify-section";
    public const string DeleteSection = "delete-section";
    #endregion

    #region Topic
    public const string DeleteTopic = "delete-topic";
    public const string DeleteTopicReply = "delete-topic-reply";
    #endregion
}