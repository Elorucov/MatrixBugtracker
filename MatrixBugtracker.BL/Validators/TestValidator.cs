using FluentValidation;
using MatrixBugtracker.BL.DTOs;

namespace MatrixBugtracker.BL.Validators
{
    public class TestValidator : AbstractValidator<TestRequestDTO>
    {
        public TestValidator()
        {
            RuleFor(p => p.Number).NotEmpty().GreaterThan(73).WithMessage("Value of the 'Number' parameter must be greater than 73");
            RuleFor(p => p.Text).NotEmpty().Length(2, 32).WithMessage("Length of the 'Text' must be between 2 and 32");
        }
    }
}
