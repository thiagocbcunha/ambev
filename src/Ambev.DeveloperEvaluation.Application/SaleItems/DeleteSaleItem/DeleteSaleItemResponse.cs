namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

/// <summary>
/// Response returned after successfully deleting a sale.
/// </summary>
public record DeleteSaleItemResponse
{
    /// <summary>
    /// Indicates whether the delete operation was successful.
    /// </summary>
    public bool Success { get; set; }
}
