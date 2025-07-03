using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.DAL.Models
{
    public record ReportFilter(List<ReportSeverity> Severities = null, List<ReportProblemType> ProblemTypes = null,
        List<ReportStatus> Statuses = null, List<Tag> Tags = null, string SearchQuery = null, bool Reverse = false);
}
