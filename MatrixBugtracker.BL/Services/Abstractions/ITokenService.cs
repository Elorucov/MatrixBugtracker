using MatrixBugtracker.BL.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ITokenService
    {
        TokenDTO GetToken(int userId);
    }
}
