using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Command for updating an existing sale.
/// </summary>
public class ChangeSaleCommand : IRequest<ChangeSaleResult>
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
    public string CustomerId { get; set; } = null!;

    /// <summary>
    /// Name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = null!;

    /// <summary>
    /// Unique identifier for the branch.
    /// </summary>
    public string BranchId { get; set; } = null!;

    /// <summary>
    /// Name of the branch.
    /// </summary>
    public string BranchName { get; set; } = null!;

    /// <summary>
    /// List of items in the sale.
    /// </summary>
    public List<ChangeSaleItemDto> Items { get; set; } = [];
}

/// <summary>
/// DTO for sale items in the ChangeSaleCommand.
/// </summary>
public class ChangeSaleItemDto
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
