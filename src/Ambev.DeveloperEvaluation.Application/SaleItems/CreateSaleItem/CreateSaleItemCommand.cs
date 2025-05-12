using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

/// <summary>
/// Command for creating a new sale.
/// </summary>
public record CreateSaleItemCommand : IRequest<CreateSaleItemResult>
{

    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    public string ProductId { get; set; } = null!;

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary>
    /// Quantity of the product in the sale.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}