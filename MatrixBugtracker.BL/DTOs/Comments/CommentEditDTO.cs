namespace MatrixBugtracker.BL.DTOs.Comments
{
    public class CommentEditDTO
    {
        public int Id { get; init; }
        public string Text { get; init; }
        public int[] FileIds { get; init; }
        public bool IsFilesPrivate { get; init; }
    }
}
