using FluentValidation;
using MatrixBugtracker.BL.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
