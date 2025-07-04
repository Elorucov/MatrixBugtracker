using FluentValidation;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Validators.Reports
{
    public class ReportPatchSeverityValidator : AbstractValidator<ReportPatchEnumDTO<ReportSeverity>>
    {
        public ReportPatchSeverityValidator()
        {
            RuleFor(p => p.Id).NotEmpty().GreaterThan(0).WithMessage(Errors.InvalidReportId);
            RuleFor(p => p.NewValue).NotEmpty();
        }
    }
}
