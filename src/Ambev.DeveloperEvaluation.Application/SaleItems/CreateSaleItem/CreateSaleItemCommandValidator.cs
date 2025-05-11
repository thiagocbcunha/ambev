using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

/// <summary>
/// Validator for CreateSaleCommand.
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleItemCommandValidator"/> class.
    /// </summary>
    public CreateSaleItemCommandValidator()
    {
    }
}
