using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Tags
{
    public class AddTagResultDTO
    {
        public int AddedCount { get; init; }
        public List<string> AlreadyExist { get; init; }
    }
}
