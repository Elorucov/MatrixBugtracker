using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class FileAdminDTO : FileDTO
    {
        public int CreatorId { get; set; }
    }
}
