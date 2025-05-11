using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// </summary>
public class SaleTests
{
    [Trait("Sale", "Entity")]
    [Fact(DisplayName = "Given valid data When creating a sale Then initializes correctly")]
    public void Given_ValidData_When_CreatingSale_Then_InitializesCorrectly()
    {
        // Arrange
        var saleNumber = 123;
        var sallerId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = "BR001";
        var branchName = "Main Branch";

        // Act
        var sale = new Sale(saleNumber, sallerId, customerId, branchId, branchName);

        // Assert
        sale.Items.Should().BeEmpty();
        sale.IsCancelled.Should().BeFalse();
        sale.SallerId.Should().Be(sallerId);
        sale.BranchId.Should().Be(branchId);
        sale.SaleNumber.Should().Be(saleNumber);
        sale.CustomerId.Should().Be(customerId);
        sale.BranchName.Should().Be(branchName);
    }

    [Trait("Sale", "Entity")]
    [Fact(DisplayName = "Given valid data When adding an item Then item is added to the sale")]
    public void Given_ValidData_When_AddingItem_Then_ItemIsAdded()
    {
        // Arrange
        var sale = new Sale(123, Guid.NewGuid(), Guid.NewGuid(), "BR001", "Main Branch");
        var productId = Guid.NewGuid().ToString();
        var productName = "Product A";
        var quantity = 5;
        var unitPrice = 10.0m;

        // Act
        sale.AddItem(productId, productName, quantity, unitPrice);

        // Assert
        var item = sale.Items.First();
        sale.Items.Should().HaveCount(1);
        item.Quantity.Should().Be(quantity);
        item.ProductId.Should().Be(productId);
        item.UnitPrice.Should().Be(unitPrice);
        item.ProductName.Should().Be(productName);
    }

    [Trait("Sale", "Entity")]
    [Fact(DisplayName = "Given a sale When cancelled Then IsCancelled is true")]
    public void Given_Sale_When_Cancelled_Then_IsCancelledIsTrue()
    {
        // Arrange
        var sale = new Sale(123, Guid.NewGuid(), Guid.NewGuid(), "BR001", "Main Branch");

        // Act
        sale.CancelSale();

        // Assert
        sale.IsCancelled.Should().BeTrue();
    }

    [Trait("Sale", "Entity")]
    [Fact(DisplayName = "Given a sale with items When calculating total amount Then returns correct total")]
    public void Given_SaleWithItems_When_CalculatingTotalAmount_Then_ReturnsCorrectTotal()
    {
        // Arrange
        var sale = new Sale(123, Guid.NewGuid(), Guid.NewGuid(), "BR001", "Main Branch");
        sale.AddItem(Guid.NewGuid().ToString(), "Product A", 2, 10.0m);
        sale.AddItem(Guid.NewGuid().ToString(), "Product B", 3, 20.0m);

        // Act
        var totalAmount = sale.TotalAmount;

        // Assert
        totalAmount.Should().Be(80.0m); // (2 * 10) + (3 * 20)
    }
}
