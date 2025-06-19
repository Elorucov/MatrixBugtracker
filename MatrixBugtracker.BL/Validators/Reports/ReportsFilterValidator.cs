using FluentValidation;
using MatrixBugtracker.BL.DTOs.Reports;

namespace MatrixBugtracker.BL.Validators.Reports
{
    public class ReportsFilterValidator : AbstractValidator<GetReportsRequestDTO>
    {
        public ReportsFilterValidator()
        {
            RuleFor(p => p.Number).GreaterThan(0);
            RuleFor(p => p.Size).GreaterThan(0);
            RuleFor(p => p.ProductId).GreaterThanOrEqualTo(0);
            RuleFor(p => p.CreatorId).GreaterThanOrEqualTo(0);
        }
    }
}
