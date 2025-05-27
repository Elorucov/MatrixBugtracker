namespace MatrixBugtracker.BL.DTOs.Tags
{
    public class AddTagResultDTO
    {
        public int AddedCount { get; init; }
        public List<string> AlreadyExist { get; init; }
    }
}
