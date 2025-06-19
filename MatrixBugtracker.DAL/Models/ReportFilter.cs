using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Models
{
    public record ReportFilter(List<ReportSeverity> Severities = null, List<ReportProblemType> ProblemTypes = null, List<ReportStatus> Statuses = null, List<Tag> Tags = null);
}
