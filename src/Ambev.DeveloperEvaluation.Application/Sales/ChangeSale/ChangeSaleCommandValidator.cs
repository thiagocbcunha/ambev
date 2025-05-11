using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Validator for ChangeSaleCommand.
/// </summary>
public class ChangeSaleCommandValidator : AbstractValidator<ChangeSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSaleCommandValidator"/> class.
    /// </summary>
    public ChangeSaleCommandValidator()
    {
        RuleFor(sale => sale.Id).NotEmpty();
        RuleFor(sale => sale.SaleNumber).GreaterThan(0);
        RuleFor(sale => sale.SaleDate).NotEmpty();
        RuleFor(sale => sale.CustomerId).NotEmpty();
        RuleFor(sale => sale.CustomerName).NotEmpty();
        RuleFor(sale => sale.BranchId).NotEmpty();
        RuleFor(sale => sale.BranchName).NotEmpty();
        RuleForEach(sale => sale.Items).SetValidator(new ChangeSaleItemValidator());
    }
}

/// <summary>
/// Validator for sale items in ChangeSaleCommand.
/// </summary>
public class ChangeSaleItemValidator : AbstractValidator<ChangeSaleItemDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSaleItemValidator"/> class.
    /// </summary>
    public ChangeSaleItemValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty();
        RuleFor(item => item.ProductName).NotEmpty();
        RuleFor(item => item.Quantity).GreaterThan(0);
        RuleFor(item => item.UnitPrice).GreaterThan(0);
    }
}
