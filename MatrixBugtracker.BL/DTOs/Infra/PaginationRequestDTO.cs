using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginationRequestDTO
    {
        public int Number { get; set; }
        public int Size { get; set; }
    }
}
