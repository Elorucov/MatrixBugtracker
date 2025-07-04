using FluentValidation;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.Validators.Products
{
    public class GetMembersRequestValidator : AbstractValidator<GetMembersRequestDTO>
    {
        public GetMembersRequestValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThan(0);
            RuleFor(p => p.PageSize).GreaterThan(0);
            RuleFor(p => p.ProductId).GreaterThan(0).WithMessage(Errors.InvalidProductId);
        }
    }
}
