using FluentValidation;
using MatrixBugtracker.BL.DTOs.Auth;

namespace MatrixBugtracker.BL.Validators.Auth
{
    public class PasswordResetValidator : AbstractValidator<PasswordResetRequestDTO>
    {
        public PasswordResetValidator()
        {
            RuleFor(p => p.Password).NotEmpty().Length(8, 128);
            RuleFor(p => p.ConfirmPassword).NotEmpty().Equal(p => p.Password).WithMessage("Password mismatch");
        }
    }
}
