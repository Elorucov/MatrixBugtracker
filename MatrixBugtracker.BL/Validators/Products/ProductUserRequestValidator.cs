using FluentValidation;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.Validators.Products
{
    public class ProductUserRequestValidator : AbstractValidator<ProductUserRequestDTO>
    {
        public ProductUserRequestValidator()
        {
            RuleFor(p => p.ProductId).GreaterThan(0).WithMessage(Errors.InvalidProductId);
            RuleFor(p => p.UserId).GreaterThan(0).WithMessage(Errors.InvalidUserId);
        }
    }
}
