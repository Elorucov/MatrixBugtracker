using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        public ResponseDTO<ReportEnumsDTO> GetEnumValues()
        {
            // TODO: Extension?
            var problemTypes = Enum.GetValues<ReportProblemType>()
                .Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"ReportProblemType_{e}"))).ToList();

            var severities = Enum.GetValues<ReportSeverity>()
                .Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"ReportSeverity_{e}"))).ToList();

            var statuses = Enum.GetValues<ReportStatus>()
                .Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"ReportStatus_{e}"))).ToList();

            ReportEnumsDTO response = new ReportEnumsDTO
            {
                ProblemTypes = problemTypes,
                Severities = severities,
                Statuses = statuses
            };

            return new ResponseDTO<ReportEnumsDTO>(response);
        }
    }
}
