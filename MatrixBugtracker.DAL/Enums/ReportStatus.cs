namespace MatrixBugtracker.DAL.Enums
{
    public enum ReportStatus : byte
    {
        Open = 0,
        InProgress = 1,
        Fixed = 2,
        Declined = 3,
        UnderReview = 4,
        Blocked = 5,
        Reopened = 6,
        CannotReproduce = 7,
        Deferred = 8,
        NeedsCorrection = 9,
        ReadyForTesting = 10,
        Verified = 11,
        WontBeFixed = 12,
        Outdated = 13,
        Duplicate = 14
    }
}
