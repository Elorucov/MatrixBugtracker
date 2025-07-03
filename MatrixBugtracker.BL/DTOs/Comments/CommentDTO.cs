using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

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

        [JsonConverter(typeof(CustomEnumConverter))]
        public ReportSeverity NewSeverity { get; set; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public ReportStatus NewStatus { get; set; }

        public List<FileDTO> Attachments { get; set; }
        public bool IsAttachmentsPrivate { get; set; }
        public bool CanDelete { get; set; }
    }
}
