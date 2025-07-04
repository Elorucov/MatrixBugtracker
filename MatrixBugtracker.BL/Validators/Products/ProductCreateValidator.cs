using FluentValidation;
using MatrixBugtracker.BL.DTOs.Products;

namespace MatrixBugtracker.BL.Validators.Products
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().Length(2, 64);
            RuleFor(p => p.Description).NotEmpty().Length(2, 256);

            RuleFor(p => p.Type).NotEmpty();
            RuleFor(p => p.AccessLevel).NotEmpty();
        }
    }
}
