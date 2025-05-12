namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Result returned after successfully retrieving a sale.
/// </summary>
public record GetSaleResult
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
    public Guid SellerId { get; set; }

    /// <summary>
    /// Name of the branch.
    /// </summary>
    public string BranchName { get; set; } = null!;

    /// <summary>
    /// Total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// List of items in the sale.
    /// </summary>
    public List<GetSaleItemDto> Items { get; set; } = new();
}

/// <summary>
/// DTO for sale items in the GetSaleResult.
/// </summary>
public class GetSaleItemDto
{
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

