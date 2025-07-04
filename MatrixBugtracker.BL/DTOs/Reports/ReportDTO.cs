using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.Domain.Enums;

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
        public ReportSeverity Severity { get; set; }
        public ReportProblemType ProblemType { get; set; }
        public ReportStatus Status { get; set; }
        public List<string> Tags { get; set; }
        public List<FileDTO> Attachments { get; set; } // only when getting single report
        public bool IsAttachmentsPrivate { get; set; }
        public ReportReproducesDTO Reproduces { get; set; } // only when getting single report
        public bool IsSeveritySetByModerator { get; set; }
        public bool CanDelete { get; set; }

        #region Extended (only when getting single user by id)

        public UserDTO Creator { get; set; }
        public ProductDTO Product { get; set; }

        #endregion
    }
}
