using FluentValidation;
using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequestDTO>
    {
        public PaginationRequestValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThan(0);
            RuleFor(p => p.PageSize).GreaterThan(0);
        }
    }
}
