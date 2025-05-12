using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetAllSaleItem;

/// <summary>
/// Validator for GetSaleCommand.
/// </summary>
public class GetAllSaleItemCommandValidator : AbstractValidator<GetAllSaleItemCommand>
{
    /// <summary>
    /// Initializes validation rules for GetSaleCommand.
    /// </summary>
    public GetAllSaleItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required.");
    }
}
