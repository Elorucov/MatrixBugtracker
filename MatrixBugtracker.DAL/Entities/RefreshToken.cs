using MatrixBugtracker.DAL.Entities.Base;

namespace MatrixBugtracker.DAL.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
