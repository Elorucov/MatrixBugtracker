using FluentValidation;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.Validators.Comments
{
    internal class CommentEditValidator : AbstractValidator<CommentEditDTO>
    {
        public CommentEditValidator()
        {
            RuleFor(p => p.Id).NotEmpty().GreaterThan(0).WithMessage(Errors.InvalidCommentId);

            RuleFor(p => p.Text).NotEmpty()
                .MinimumLength(1).WithMessage(Errors.TooShort)
                .MaximumLength(2048).WithMessage(Errors.TooLong);
        }
    }
}
