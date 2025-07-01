using MatrixBugtracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class UserNotificationDTO : NotificationDTO
    {
        public UserNotificationKind Kind { get; set; }
        public LinkedEntityType LinkedEntityType { get; set; }
        public int LinkedEntityId { get; set; }
    }
}
