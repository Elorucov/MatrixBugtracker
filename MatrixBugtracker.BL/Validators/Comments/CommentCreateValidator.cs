using FluentValidation;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.Validators.Comments
{
    internal class CommentCreateValidator : AbstractValidator<CommentCreateDTO>
    {
        public CommentCreateValidator()
        {
            RuleFor(p => p.ReportId).NotEmpty().GreaterThan(0).WithMessage(Errors.InvalidReportId);

            RuleFor(p => p.Text).NotEmpty()
                .MinimumLength(1).WithMessage(Errors.TooShort)
                .MaximumLength(2048).WithMessage(Errors.TooLong);

            RuleFor(p => p.FileIds).Must(x => x.Length <= 5);
        }
    }
}
