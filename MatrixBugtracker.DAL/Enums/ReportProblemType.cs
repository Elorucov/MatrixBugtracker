namespace MatrixBugtracker.DAL.Enums
{
    public enum ReportProblemType : byte
    {
        Suggestion = 1,
        AppCrash = 2,
        AppFroze = 3,
        FunctionNotWorking = 4,
        DataDamage = 5,
        Performance = 6,
        AesteticDiscrepancies = 7,
        Typo = 8
    }
}
