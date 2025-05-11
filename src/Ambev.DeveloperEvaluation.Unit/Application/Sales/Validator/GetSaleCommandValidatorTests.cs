using Xunit;
using FluentValidation.TestHelper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Validator;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleCommandValidator"/> class.
/// </summary>
public class GetSaleCommandValidatorTests
{
    private readonly GetSaleCommandValidator _validator;

    public GetSaleCommandValidatorTests()
    {
        _validator = new GetSaleCommandValidator();
    }

    [Trait("Sale", "Validator")]
    [Fact(DisplayName = "Valid GetSaleCommand should pass validation")]
    public void Given_ValidGetSaleCommand_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new GetSaleCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Trait("Sale", "Validator")]
    [Fact(DisplayName = "Invalid GetSaleCommand should fail validation")]    
    public void Given_InvalidGetSaleCommand_When_Validated_Then_ShouldHaveErrors()
    {
        // Arrange
        var command = new GetSaleCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}
