namespace MatrixBugtracker.BL.DTOs.Comments
{
    public class CommentCreateDTO
    {
        public int ReportId { get; init; }
        public string Text { get; init; }
        public int[] FileIds { get; init; }
        public bool IsFilesPrivate { get; init; }
        public bool AsModerator { get; init; }
    }
}
