using FluentValidation;
using MatrixBugtracker.BL.DTOs.Users;

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
