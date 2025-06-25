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
    public class ReportPatchStatusValidator : AbstractValidator<ReportPatchEnumDTO<ReportStatus>>
    {
        public ReportPatchStatusValidator()
        {
            RuleFor(p => p.Id).NotEmpty().GreaterThan(0);

            RuleFor(p => p.NewValue).NotEmpty()
                .IsInEnum().WithMessage(string.Format(Errors.InvalidEnum, EnumExtensions.GetValuesCommaSeparated<ReportStatus>()));
        }
    }
}