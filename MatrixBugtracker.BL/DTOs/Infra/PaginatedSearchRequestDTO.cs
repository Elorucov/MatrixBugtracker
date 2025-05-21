using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginatedSearchRequestDTO : PaginationRequestDTO
    {
        public string Query { get; init; }
    }
}
