using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;

namespace MatrixBugtracker.BL.DTOs.Users
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public FileDTO Photo { get; set; }


        #region Extended (only when getting single user by id)

        public UserStatsDTO Stats { get; set; }
        public List<ProductDTO> MentionedProducts { get; set; }

        #endregion
    }
}
