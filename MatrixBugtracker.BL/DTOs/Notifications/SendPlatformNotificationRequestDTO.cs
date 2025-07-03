using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class SendPlatformNotificationRequestDTO
    {
        [JsonConverter(typeof(CustomEnumConverter))]
        public PlatformNotificationKind Kind { get; init; }

        public string Text { get; init; }
    }
}
