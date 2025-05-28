using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IReportsService
    {
        ResponseDTO<ReportEnumsDTO> GetEnumValues();
    }
}
