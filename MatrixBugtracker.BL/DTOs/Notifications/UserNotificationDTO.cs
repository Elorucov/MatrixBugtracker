using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class UserNotificationDTO : NotificationDTO
    {
        [JsonConverter(typeof(CustomEnumConverter))]
        public UserNotificationKind Kind { get; set; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public LinkedEntityType? LinkedEntityType { get; set; }

        public int? LinkedEntityId { get; set; }
    }
}
