using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Command for updating an existing sale.
/// </summary>
public record ChangeSaleCommand : IRequest<ChangeSaleResult>
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
    /// Unique identifier for the customer.
    /// </summary>
    public Guid SallerId { get; set; }

    /// <summary>
    /// Unique identifier for the branch.
    /// </summary>
    public string BranchId { get; set; } = null!;

    /// <summary>
    /// Name of the branch.
    /// </summary>
    public string BranchName { get; set; } = null!;
}