using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IReportsService
    {
        Task<ResponseDTO<int?>> CreateAsync(ReportCreateDTO request);
        Task<PaginationResponseDTO<ReportDTO>> GetAsync(GetReportsRequestDTO request);
        Task<ResponseDTO<ReportDTO>> GetByIdAsync(int reportId);
        ResponseDTO<ReportEnumsDTO> GetEnumValues();
    }
}
