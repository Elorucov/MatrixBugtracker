using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PageWithMentionsDTO<T> : PageDTO<T>
    {
        public PageWithMentionsDTO(List<T> items, int? count) : base(items, count) { }

        public List<UserDTO> MentionedUsers { get; init; }
        public List<ProductDTO> MentionedProducts { get; init; }
    }
}
