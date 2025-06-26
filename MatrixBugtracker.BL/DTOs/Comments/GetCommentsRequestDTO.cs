using MatrixBugtracker.BL.DTOs.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Comments
{
    public class GetCommentsRequestDTO : PaginationRequestDTO
    {
        public int ReportId { get; init; }
    }
}
