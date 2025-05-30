using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IReportsService
    {
        Task<ResponseDTO<int?>> CreateAsync(ReportCreateDTO request);
        ResponseDTO<ReportEnumsDTO> GetEnumValues();
    }
}
