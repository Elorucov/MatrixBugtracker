using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Admin
{
    public class SetRoleRequestDTO
    {
        public int UserId { get; set; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public UserRole Role { get; set; }
    }
}
