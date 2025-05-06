using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Auth
{
    public class TokenDTO
    {
        public string Token { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}
