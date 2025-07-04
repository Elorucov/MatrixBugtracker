using FluentValidation;
using MatrixBugtracker.BL.DTOs.Users;

namespace MatrixBugtracker.BL.Validators.Users
{
    public class GetUsersByRoleValidator : AbstractValidator<GetUsersByRoleRequestDTO>
    {
        public GetUsersByRoleValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThan(0);
            RuleFor(p => p.PageSize).GreaterThan(0);
            RuleFor(p => p.Role).NotEmpty();
        }
    }
}
