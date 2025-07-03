using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IReportsService
    {
        Task<ResponseDTO<Report>> CheckAccessAsync(int reportId, bool needIncludes = false);
        Task<ResponseDTO<int?>> CreateAsync(ReportCreateDTO request);
        Task<ResponseDTO<bool>> EditAsync(ReportEditDTO request);
        Task<ResponseDTO<bool>> SetSeverityAsync(ReportPatchEnumDTO<ReportSeverity> request);
        Task<ResponseDTO<bool>> SetStatusAsync(ReportPatchEnumDTO<ReportStatus> request);
        Task<ResponseDTO<bool>> SetReproducedAsync(int reportId, bool reproduced);
        Task<ResponseDTO<List<UserDTO>>> GetReproducedUsersAsync(int reportId);
        Task<PaginationResponseDTO<ReportDTO>> GetAsync(GetReportsRequestDTO request);
        Task<ResponseDTO<ReportDTO>> GetByIdAsync(int reportId);
        Task<ResponseDTO<bool>> DeleteAsync(int reportId);
    }
}
