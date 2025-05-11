namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Result returned after successfully creating a sale.
/// </summary>
public class CreateSaleResult
{
    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number, which is a unique identifier for the sale within the system.
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// The date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The name of the customer associated with the sale.
    /// </summary>
    public string CustomerName { get; set; } = null!;

    /// <summary>
    /// The name of the branch where the sale was made.
    /// </summary>
    public string BranchName { get; set; } = null!;

    /// <summary>
    /// The name of the product sold in the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
