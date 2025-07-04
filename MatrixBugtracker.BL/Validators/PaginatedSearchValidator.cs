using FluentValidation;
using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Validators
{
    public class PaginatedSearchValidator : AbstractValidator<PaginatedSearchRequestDTO>
    {
        public PaginatedSearchValidator()
        {
            RuleFor(p => p.Query).NotEmpty();
            RuleFor(p => p.Number).GreaterThan(0);
            RuleFor(p => p.Size).GreaterThan(0);
        }
    }
}
