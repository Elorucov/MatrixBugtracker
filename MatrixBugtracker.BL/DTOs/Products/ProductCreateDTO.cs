using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductType Type { get; set; }
        public ProductAccessLevel AccessLevel { get; set; }
        public int? PhotoFileId { get; set; }
    }
}
