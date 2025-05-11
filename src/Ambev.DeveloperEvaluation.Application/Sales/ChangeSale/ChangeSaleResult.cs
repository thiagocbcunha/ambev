namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Result returned after successfully updating a sale.
/// </summary>
public record ChangeSaleResult
{
    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sale number.
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// Date of the sale.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Unique identifier for the customer.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Unique identifier for the seller.
    /// </summary>
    public Guid SallerId { get; set; }

    /// <summary>
    /// Unique identifier for the branch.
    /// </summary>
    public string BranchName { get; set; } = null!;

    /// <summary>
    /// List of items in the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
