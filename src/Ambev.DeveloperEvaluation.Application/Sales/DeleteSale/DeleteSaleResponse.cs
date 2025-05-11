namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Response returned after successfully deleting a sale.
/// </summary>
public class DeleteSaleResponse
{
    /// <summary>
    /// Indicates whether the delete operation was successful.
    /// </summary>
    public bool Success { get; set; }
}
