using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportEditDTO
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Steps { get; init; }
        public string Actual { get; init; }
        public string Supposed { get; init; }
        public ReportSeverity Severity { get; init; }
        public ReportProblemType ProblemType { get; init; }
        public string[] Tags { get; init; }
        public int[] FileIds { get; init; }
        public bool IsFilesPrivate { get; init; }
    }
}
