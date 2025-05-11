namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Response returned after successfully deleting a sale.
/// </summary>
public class DeleteSaleResult
{
    /// <summary>
    /// Indicates whether the delete operation was successful.
    /// </summary>
    public bool Success { get; set; }
}
