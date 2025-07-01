using MatrixBugtracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class PlatformNotificationDTO : NotificationDTO
    {
        public PlatformNotificationKind Kind { get; set; }
    }
}
