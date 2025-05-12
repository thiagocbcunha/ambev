using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// </summary>
public class SaleItemTests
{
    [Trait("SaleItem", "Entity")]
    [Fact(DisplayName = "Given valid data When creating a sale item Then initializes correctly")]
    public void Given_ValidData_When_CreatingSaleItem_Then_InitializesCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid().ToString();
        var productName = "Product A";
        var quantity = 5;
        var unitPrice = 10.0m;

        // Act
        var saleItem = new SaleItem(productId, productName, quantity, unitPrice);

        // Assert
        saleItem.ProductId.Should().Be(productId);
        saleItem.ProductName.Should().Be(productName);
        saleItem.Quantity.Should().Be(quantity);
        saleItem.UnitPrice.Should().Be(unitPrice);
        saleItem.Discount.Should().Be(5);
    }

    [Trait("SaleItem", "Entity")]
    [Fact(DisplayName = "Given valid data When updating a sale item Then updates correctly")]
    public void Given_ValidData_When_UpdatingSaleItem_Then_UpdatesCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid().ToString();
        var saleItem = new SaleItem(Guid.NewGuid().ToString(), "Product A", 5, 10.0m);
        var newProductName = "Product B";
        var newQuantity = 10;
        var newUnitPrice = 20.0m;

        // Act
        saleItem.Patch(productId, newProductName, newQuantity, newUnitPrice);

        // Assert
        saleItem.ProductId.Should().Be(productId);
        saleItem.Quantity.Should().Be(newQuantity);
        saleItem.UnitPrice.Should().Be(newUnitPrice);
        saleItem.ProductName.Should().Be(newProductName);
    }

    [Trait("SaleItem", "Entity")]
    [Fact(DisplayName = "Given valid quantity When calculating discount Then applies correct discount")]
    public void Given_ValidQuantity_When_CalculatingDiscount_Then_AppliesCorrectDiscount()
    {
        // Arrange
        var saleItem = new SaleItem(Guid.NewGuid().ToString(), "Product A", 10, 10.0m);

        // Act
        saleItem.Patch(saleItem.ProductId, saleItem.ProductName, saleItem.Quantity, saleItem.UnitPrice);

        // Assert
        saleItem.Discount.Should().Be(20.0m); // 10 * 10 * 0.2
    }
}
