using FluentValidation;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.BL.Validators.Reports
{
    public class ReportPatchSeverityValidator : AbstractValidator<ReportPatchEnumDTO<ReportSeverity>>
    {
        public ReportPatchSeverityValidator()
        {
            RuleFor(p => p.Id).NotEmpty().GreaterThan(0);

            RuleFor(p => p.NewValue).NotEmpty()
                .IsInEnum().WithMessage(string.Format(Errors.InvalidEnum, EnumExtensions.GetValuesCommaSeparated<ReportSeverity>()));
        }
    }
}
