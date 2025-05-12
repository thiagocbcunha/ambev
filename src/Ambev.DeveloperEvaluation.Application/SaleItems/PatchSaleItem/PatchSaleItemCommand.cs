using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.PatchSaleItem;

/// <summary>
/// DTO for sale items in the ChangeSaleCommand.
/// </summary>
public record PatchSaleItemCommand : IRequest<PatchSaleItemResult>
{
    /// <summary>
    /// Unique identifier for the sale Item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    public string? ProductId { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Quantity of the product in the sale.
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal? UnitPrice { get; set; }
}
