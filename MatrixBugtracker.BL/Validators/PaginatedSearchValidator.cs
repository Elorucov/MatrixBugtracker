using FluentValidation;
using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Validators
{
    public class PaginatedSearchValidator : AbstractValidator<PaginatedSearchRequestDTO>
    {
        public PaginatedSearchValidator()
        {
            RuleFor(p => p.SearchQuery).NotEmpty();
            RuleFor(p => p.PageNumber).GreaterThan(0);
            RuleFor(p => p.PageSize).GreaterThan(0);
        }
    }
}
