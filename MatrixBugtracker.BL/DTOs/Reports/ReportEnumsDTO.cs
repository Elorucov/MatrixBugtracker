using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportEnumsDTO
    {
        public List<EnumValueDTO> ProblemTypes { get; init; }
        public List<EnumValueDTO> Severities { get; init; }
        public List<EnumValueDTO> Statuses { get; init; }
    }
}
