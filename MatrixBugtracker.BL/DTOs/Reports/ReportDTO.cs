using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Steps { get; set; }
        public string Actual { get; set; }
        public string Supposed { get; set; }
        public EnumValueDTO Severity { get; set; }
        public EnumValueDTO ProblemType { get; set; }
        public EnumValueDTO Status { get; set; }
        public List<string> Tags { get; set; }
        public List<FileDTO> Attachments { get; set; } // only when getting single report
        public bool IsAttachmentsPrivate { get; set; }
        public ReportReproducesDTO Reproduces { get; set; } // only when getting single report
        public bool IsSeveritySetByModerator { get; set; }
        public bool CanDelete { get; set; }
    }
}
