using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Users
{
    public class UserStatsDTO
    {
        public int TotalReports { get; init; }
        public int AcceptedReports { get; init; }
        public int JoinedProducts { get; init; }
        public List<UserStatProductDTO> TopProducts { get; init; }
    }
}
