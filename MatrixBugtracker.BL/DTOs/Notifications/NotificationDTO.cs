using MatrixBugtracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public string Text { get; set; }
    }
}
