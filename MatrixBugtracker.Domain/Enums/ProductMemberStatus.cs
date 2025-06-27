namespace MatrixBugtracker.Domain.Enums
{
    public enum ProductMemberStatus : byte
    {
        NotMember = 0,
        JoinRequested = 1,
        InviteReceived = 2,
        Joined = 3
    }
}
