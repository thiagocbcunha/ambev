using Xunit;
using FluentValidation.TestHelper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Validator;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleItemValidator"/> class.
/// </summary>
public class CreateSaleItemValidatorTests
{
    private readonly CreateSaleItemValidator _validator;

    public CreateSaleItemValidatorTests()
    {
        _validator = new CreateSaleItemValidator();
    }

    [Trait("Sale", "Validator")]
    [Fact(DisplayName = "Valid CreateSaleItemDto should pass validation")]
    public void Given_ValidCreateSaleItemDto_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var item = new CreateSaleItemDto
        {
            ProductId = Guid.NewGuid().ToString(),
            ProductName = "Product A",
            Quantity = 1,
            UnitPrice = 10.0m
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Trait("Sale", "Validator")]
    [Fact(DisplayName = "Invalid CreateSaleItemDto should fail validation")]
    public void Given_InvalidCreateSaleItemDto_When_Validated_Then_ShouldHaveErrors()
    {
        // Arrange
        var item = new CreateSaleItemDto
        {
            ProductId = null!,
            ProductName = string.Empty,
            Quantity = 0,
            UnitPrice = 0.0m
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(i => i.ProductId);
        result.ShouldHaveValidationErrorFor(i => i.ProductName);
        result.ShouldHaveValidationErrorFor(i => i.Quantity);
        result.ShouldHaveValidationErrorFor(i => i.UnitPrice);
    }
}
