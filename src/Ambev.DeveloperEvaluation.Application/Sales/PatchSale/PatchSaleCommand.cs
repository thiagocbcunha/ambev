using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.PatchSale;

/// <summary>
/// Command for updating an existing sale.
/// </summary>
public record PatchSaleCommand : IRequest<PatchSaleResult>
{
    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sale number.
    /// </summary>
    public int? SaleNumber { get; set; }

    /// <summary>
    /// Unique identifier for the customer.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Unique identifier for the customer.
    /// </summary>
    public Guid? SallerId { get; set; }

    /// <summary>
    /// Unique identifier for the branch.
    /// </summary>
    public string? BranchId { get; set; }

    /// <summary>
    /// Name of the branch.
    /// </summary>
    public string? BranchName { get; set; }
}