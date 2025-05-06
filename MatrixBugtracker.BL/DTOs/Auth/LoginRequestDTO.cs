using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Auth
{
    public class LoginRequestDTO
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
