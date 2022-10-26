using Dukkantek.Test.Domain.Enums;
using FluentValidation;

namespace Dukkantek.Test.Application.Products.Commands.ChangeProductStatus;

public class ChangeProductStatusRequestValidator : AbstractValidator<ChangeProductStatusRequest>
{
    public ChangeProductStatusRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("A valid product is required.");

        RuleFor(x => x.StatusId)
            .Must(status => Enum.IsDefined(typeof(ProductStatus), status))
            .WithMessage("Product status is out of range.");
    }
}
