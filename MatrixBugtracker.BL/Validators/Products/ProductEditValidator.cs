using FluentValidation;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.Validators.Products
{
    public class ProductEditValidator : AbstractValidator<ProductEditDTO>
    {
        public ProductEditValidator()
        {
            RuleFor(p => p.Id).NotEmpty().GreaterThan(0).WithMessage(Errors.InvalidProductId);
            RuleFor(p => p.Name).NotEmpty().Length(2, 64);
            RuleFor(p => p.Description).NotEmpty().Length(2, 256);
        }
    }
}
