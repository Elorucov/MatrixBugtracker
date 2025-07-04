using FluentValidation;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.Validators.Reports
{
    public class ReportsFilterValidator : AbstractValidator<GetReportsRequestDTO>
    {
        public ReportsFilterValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThan(0);
            RuleFor(p => p.PageSize).GreaterThan(0);
            RuleFor(p => p.ProductId).GreaterThanOrEqualTo(0).WithMessage(Errors.InvalidProductId);
            RuleFor(p => p.CreatorId).GreaterThanOrEqualTo(0).WithMessage(Errors.InvalidUserId);
        }
    }
}
