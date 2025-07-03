using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportCreateDTO
    {
        public int ProductId { get; init; }
        public string Title { get; init; }
        public string Steps { get; init; }
        public string Actual { get; init; }
        public string Supposed { get; init; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ReportSeverity Severity { get; init; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ReportProblemType ProblemType { get; init; }

        public string[] Tags { get; init; }
        public int[] FileIds { get; init; }
        public bool IsFilesPrivate { get; init; }
    }
}
