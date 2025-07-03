using FluentValidation;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.Validators.Reports
{
    public class ReportCreateValidator : AbstractValidator<ReportCreateDTO>
    {
        public ReportCreateValidator()
        {
            RuleFor(p => p.ProductId).NotEmpty().GreaterThan(0).WithMessage(Errors.InvalidProductId);

            RuleFor(p => p.Title).NotEmpty()
                .MinimumLength(3).WithMessage(Errors.TooShort)
                .MaximumLength(128).WithMessage(Errors.TooLong);

            RuleFor(p => p.Steps).NotEmpty()
                .MinimumLength(10).WithMessage(Errors.TooShort)
                .MaximumLength(4096).WithMessage(Errors.TooLong);

            RuleFor(p => p.Actual).NotEmpty()
                .MinimumLength(10).WithMessage(Errors.TooShort)
                .MaximumLength(4096).WithMessage(Errors.TooLong);

            RuleFor(p => p.Supposed).NotEmpty()
                .MinimumLength(10).WithMessage(Errors.TooShort)
                .MaximumLength(4096).WithMessage(Errors.TooLong);

            RuleFor(p => p.FileIds).Must(x => x.Length <= 5);
        }
    }
}
