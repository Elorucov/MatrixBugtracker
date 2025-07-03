using FluentValidation;
using MatrixBugtracker.BL.DTOs.Admin;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Validators.Admin
{
    public class SetRoleRequestValidator : AbstractValidator<SetRoleRequestDTO>
    {
        public SetRoleRequestValidator()
        {
            RuleFor(p => p.UserId).GreaterThan(0);

            RuleFor(p => p.Role).IsInEnum().WithMessage(string.Format(Errors.InvalidEnum, EnumExtensions.GetStringValuesCommaSeparated<UserRole>()));
        }
    }
}
