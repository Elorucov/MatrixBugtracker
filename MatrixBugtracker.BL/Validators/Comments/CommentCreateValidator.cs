using FluentValidation;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Validators.Comments
{
    internal class CommentCreateValidator : AbstractValidator<CommentCreateDTO>
    {
        public CommentCreateValidator()
        {
            RuleFor(p => p.ReportId).NotEmpty().GreaterThan(0).WithMessage(Errors.InvalidReportId);

            RuleFor(p => p.Text).NotEmpty()
                .MinimumLength(1).WithMessage(Errors.TooShort)
                .MaximumLength(2048).WithMessage(Errors.TooShort);
        }
    }
}
