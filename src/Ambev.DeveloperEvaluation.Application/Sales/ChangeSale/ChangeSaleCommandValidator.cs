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
        RuleFor(sale => sale.SallerId).NotEmpty();
        RuleFor(sale => sale.BranchId).NotEmpty();
        RuleFor(sale => sale.BranchName).NotEmpty();
    }
}
