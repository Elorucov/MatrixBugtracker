using FluentValidation;
using MatrixBugtracker.BL.DTOs;
using MatrixBugtracker.BL.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Validators.Auth
{
    public class RegistrationValidator : AbstractValidator<RegisterRequestDTO>
    {
        public RegistrationValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().Length(2, 32);
            RuleFor(p => p.LastName).NotEmpty().Length(2, 32);
            RuleFor(p => p.Email).NotEmpty().Length(6, 255).EmailAddress().WithMessage("Invalid email");
            RuleFor(p => p.Password).NotEmpty().Length(8, 128);
            RuleFor(p => p.ConfirmPassword).NotEmpty().Equal(p => p.Password).WithMessage("Password mismatch");
        }
    }
}
