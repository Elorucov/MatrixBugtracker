using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class GetReportsRequestDTO : PaginatedSearchRequestDTO
    {
        public int CreatorId { get; init; }
        public int ProductId { get; init; }
        public List<ReportSeverity> Severities { get; init; }
        public List<ReportProblemType> ProblemTypes { get; init; }
        public List<ReportStatus> Statuses { get; init; }
        public string[] Tags { get; init; }
    }
}
