using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportReproducesDTO
    {
        public int Count { get; init; }
        public bool IsReproducedByMe { get; init; }
    }
}
