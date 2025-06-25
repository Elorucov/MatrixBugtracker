using FluentValidation;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Validators.Reports
{
    public class ReportEditValidator : AbstractValidator<ReportEditDTO>
    {
        public ReportEditValidator()
        {
            RuleFor(p => p.Id).NotEmpty().GreaterThan(0).WithMessage(Errors.InvalidReportId);

            RuleFor(p => p.Title).NotEmpty()
                .MinimumLength(3).WithMessage(Errors.TooLong)
                .MaximumLength(128).WithMessage(Errors.TooShort);

            RuleFor(p => p.Steps).NotEmpty()
                .MinimumLength(10).WithMessage(Errors.TooLong)
                .MaximumLength(4096).WithMessage(Errors.TooShort);

            RuleFor(p => p.Actual).NotEmpty()
                .MinimumLength(10).WithMessage(Errors.TooLong)
                .MaximumLength(4096).WithMessage(Errors.TooShort);

            RuleFor(p => p.Supposed).NotEmpty()
                .MinimumLength(10).WithMessage(Errors.TooLong)
                .MaximumLength(4096).WithMessage(Errors.TooShort);

            RuleFor(p => p.Severity).NotEmpty()
                .IsInEnum().WithMessage(string.Format(Errors.InvalidEnum, EnumExtensions.GetValuesCommaSeparated<ReportSeverity>()));

            RuleFor(p => p.ProblemType).NotEmpty()
                .IsInEnum().WithMessage(string.Format(Errors.InvalidEnum, EnumExtensions.GetValuesCommaSeparated<ReportProblemType>()));
        }
    }
}
