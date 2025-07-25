﻿using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.Domain.Enums;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class PlatformNotificationDTO : NotificationDTO
    {
        [JsonConverter(typeof(CustomEnumConverter))]
        public PlatformNotificationKind Kind { get; set; }
    }
}
