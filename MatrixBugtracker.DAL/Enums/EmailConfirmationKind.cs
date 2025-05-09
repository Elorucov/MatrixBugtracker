using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Enums
{
    public enum EmailConfirmationKind : byte
    {
        Registration = 1,
        PasswordReset = 2
    }
}
