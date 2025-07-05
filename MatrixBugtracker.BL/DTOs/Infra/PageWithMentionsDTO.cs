using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Users;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PageWithMentionsDTO<T> : PageDTO<T>
    {
        public PageWithMentionsDTO(List<T> items, int? count) : base(items, count) { }

        public List<UserDTO> MentionedUsers { get; init; }
        public List<ProductDTO> MentionedProducts { get; init; }
    }
}
