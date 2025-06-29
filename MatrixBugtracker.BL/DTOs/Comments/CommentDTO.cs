using MatrixBugtracker.BL.DTOs.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Comments
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public CommentAuthorDTO Author { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int ReportId { get; set; }
        public string Text { get; set; }
        public EnumValueDTO NewSeverity { get; set; }
        public EnumValueDTO NewStatus { get; set; }
        public List<FileDTO> Attachments { get; set; }
        public bool IsAttachmentsPrivate { get; set; }
        public bool CanDelete { get; set; }
    }
}
