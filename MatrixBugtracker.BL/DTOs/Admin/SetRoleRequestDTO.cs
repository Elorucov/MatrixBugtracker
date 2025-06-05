using MatrixBugtracker.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Admin
{
    public class SetRoleRequestDTO
    {
        public int UserId { get; set; }
        public UserRole Role { get; set; }
    }
}
