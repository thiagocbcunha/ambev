using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.PatchSale;

/// <summary>
/// Validator for ChangeSaleCommand.
/// </summary>
public class PatchSaleCommandValidator : AbstractValidator<PatchSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatchSaleCommandValidator"/> class.
    /// </summary>
    public PatchSaleCommandValidator()
    {
        RuleFor(sale => sale.Id)
            .NotEmpty();
    }
}
