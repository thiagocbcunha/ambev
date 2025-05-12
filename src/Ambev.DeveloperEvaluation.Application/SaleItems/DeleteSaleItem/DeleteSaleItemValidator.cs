using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

/// <summary>
/// Validator for DeleteSaleCommand.
/// </summary>
public class DeleteSaleItemValidator : AbstractValidator<DeleteSaleItemCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteSaleCommand.
    /// </summary>
    public DeleteSaleItemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}
