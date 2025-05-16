using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IProductService
    {
        Task<ResponseDTO<int>> CreateAsync(ProductCreateDTO request);
        Task<ResponseDTO<bool>> EditAsync(ProductEditDTO request);
        ResponseDTO<ProductEnumsDTO> GetEnumValues();
    }
}
