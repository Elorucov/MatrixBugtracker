namespace MatrixBugtracker.DAL.Enums
{
    public enum UserNotificationKind : byte
    {
        ProductInvitation = 1,
        ProductJoinAccepted = 2,
        ProductTestingFinished = 3,
        ReportCommentAdded = 4,
        PasswordReset = 5,
        RoleChanged = 6
    }
}
