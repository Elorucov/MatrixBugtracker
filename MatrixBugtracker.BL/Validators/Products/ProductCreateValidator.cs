using FluentValidation;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Validators.Products
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().Length(2, 64);
            RuleFor(p => p.Description).NotEmpty().Length(2, 256);

            RuleFor(p => p.Type).NotEmpty()
                .IsInEnum().WithMessage(string.Format(Errors.InvalidEnum, EnumExtensions.GetValuesCommaSeparated<ProductType>()));

            RuleFor(p => p.AccessLevel).NotEmpty()
                .IsInEnum().WithMessage(string.Format(Errors.InvalidEnum, EnumExtensions.GetValuesCommaSeparated<ProductAccessLevel>()));
        }
    }
}
