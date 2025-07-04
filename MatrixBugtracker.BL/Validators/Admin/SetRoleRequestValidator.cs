using FluentValidation;
using MatrixBugtracker.BL.DTOs.Admin;

namespace MatrixBugtracker.BL.Validators.Admin
{
    public class SetRoleRequestValidator : AbstractValidator<SetRoleRequestDTO>
    {
        public SetRoleRequestValidator()
        {
            RuleFor(p => p.UserId).GreaterThan(0);
            RuleFor(p => p.Role).NotEmpty();
        }
    }
}
