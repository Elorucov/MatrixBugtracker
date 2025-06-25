using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportPatchEnumDTO<T> where T: struct, Enum 
    {
        public int Id { get; set; }
        public T NewValue { get; set; }
        public string? Comment { get; set; }
    }
}
