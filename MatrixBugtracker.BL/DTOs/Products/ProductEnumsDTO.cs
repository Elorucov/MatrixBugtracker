using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class ProductEnumsDTO
    {
        public List<EnumValueDTO> Types { get; init; }
        public List<EnumValueDTO> AccessLevels { get; init; }
    }
}
