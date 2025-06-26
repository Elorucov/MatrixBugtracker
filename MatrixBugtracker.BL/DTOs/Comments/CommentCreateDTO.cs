using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Comments
{
    public class CommentCreateDTO
    {
        public int ReportId { get; init; }
        public string Text { get; init; }
        public int[] FileIds { get; init; }
        public bool IsFilesPrivate { get; init; }
    }
}
