namespace MatrixBugtracker.DAL.Enums
{
    public enum UserNotificationKind : byte
    {
        ProductInvitation,
        ProductJoinAccepted,
        ProductTestingFinished,
        ReportCommentAdded,
        PasswordReset,
        RoleChanged
    }
}
