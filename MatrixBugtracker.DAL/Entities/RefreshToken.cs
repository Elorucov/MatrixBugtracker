using MatrixBugtracker.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
