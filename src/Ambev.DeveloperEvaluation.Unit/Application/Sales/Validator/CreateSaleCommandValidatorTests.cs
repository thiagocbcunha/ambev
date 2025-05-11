using Xunit;
using FluentValidation.TestHelper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Validator;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleCommandValidator"/> class.
/// </summary>
public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator;

    public CreateSaleCommandValidatorTests()
    {
        _validator = new CreateSaleCommandValidator();
    }

    [Trait("Sale", "Validator")]
    [Fact(DisplayName = "Valid CreateSaleCommand should pass validation")]
    public void Given_ValidCreateSaleCommand_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = 123,
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            SallerId = Guid.NewGuid(),
            BranchId = "BR001",
            BranchName = "Main Branch",
            Items =
            [
                new()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 1,
                    UnitPrice = 10.0m
                }
            ]
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Trait("Sale", "Validator")]
    [Fact(DisplayName = "Invalid CreateSaleCommand should fail validation")]
    public void Given_InvalidCreateSaleCommand_When_Validated_Then_ShouldHaveErrors()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = 0,
            SaleDate = default,
            CustomerId = Guid.Empty,
            SallerId = Guid.Empty,
            BranchId = string.Empty,
            BranchName = string.Empty,
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.SaleNumber);
        result.ShouldHaveValidationErrorFor(c => c.SaleDate);
        result.ShouldHaveValidationErrorFor(c => c.CustomerId);
        result.ShouldHaveValidationErrorFor(c => c.SallerId);
        result.ShouldHaveValidationErrorFor(c => c.BranchId);
        result.ShouldHaveValidationErrorFor(c => c.BranchName);
    }
}
