using FluentValidation;
using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequestDTO>
    {
        public PaginationRequestValidator()
        {
            RuleFor(p => p.Number).GreaterThan(0);
            RuleFor(p => p.Size).GreaterThan(0);
        }
    }
}
