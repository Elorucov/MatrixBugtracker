using FluentValidation;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Validators.Users
{
    public class UserEditValidator : AbstractValidator<UserEditDTO>
    {
        public UserEditValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().Length(2, 32);
            RuleFor(p => p.LastName).NotEmpty().Length(2, 32);
        }
    }
}
