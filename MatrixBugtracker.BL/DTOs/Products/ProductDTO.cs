using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FileDTO Photo { get; set; }
        public ProductAccessLevel AccessLevel { get; set; }
        public ProductType Type { get; set; }
        public ProductMemberStatus MembershipStatus { get; set; }
        public bool IsOver { get; set; }
    }
}
