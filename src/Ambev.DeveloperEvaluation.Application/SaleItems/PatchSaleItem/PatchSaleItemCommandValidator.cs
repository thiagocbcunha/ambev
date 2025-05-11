using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.PatchSaleItem;

/// <summary>
/// Validator for sale items in ChangeSaleCommand.
/// </summary>
public class ChangeSaleItemValidator : AbstractValidator<PatchSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSaleItemValidator"/> class.
    /// </summary>
    public ChangeSaleItemValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty();
        RuleFor(item => item.ProductName).NotEmpty();
        RuleFor(item => item.Quantity).GreaterThan(0)
            .LessThanOrEqualTo(SaleItem.MaxQuantity);
        RuleFor(item => item.UnitPrice).GreaterThan(0);
    }
}
