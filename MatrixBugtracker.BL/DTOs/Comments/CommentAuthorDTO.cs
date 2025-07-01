namespace MatrixBugtracker.BL.DTOs.Comments
{
    // Used for display moderator name instead real user name for moder's comments.
    // Other moders and higher roles can get real user id, tester not (e. g. UserId = null)
    public class CommentAuthorDTO
    {
        public int? UserId { get; set; }
        public string Name { get; set; }
    }
}
