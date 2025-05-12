using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
public record CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// The date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The unique identifier of the customer associated with the sale.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The unique identifier of the seller associated with the sale.
    /// </summary>
    public Guid SallerId { get; set; }

    /// <summary>
    /// The unique identifier of the branch where the sale was made.
    /// </summary>
    public string BranchId { get; set; } = null!;

    /// <summary>
    /// The name of the branch where the sale was made.
    /// </summary>
    public string BranchName { get; set; } = null!;

    /// <summary>
    /// The total amount of the sale.
    /// </summary>
    public List<CreateSaleItemDto> Items { get; set; } = [];
}

/// <summary>
/// DTO for sale items in the CreateSaleCommand.
/// </summary>
public record CreateSaleItemDto
{
    /// <summary>
    /// Unique identifier for the product in the sale item.
    /// </summary>
    public required string ProductId { get; set; }

    /// <summary>
    /// The name of the product in the sale item.
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary>
    /// The quantity of the product sold in the sale item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product in the sale item.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
