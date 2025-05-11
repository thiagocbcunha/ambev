using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for SaleItem entity operations
/// </summary>
public interface ISaleItemRepository : IRepository<SaleItem>
{
    /// <summary>
    /// Retrieves a list of SaleItems filtered by SaleId and ProductName.
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="productName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<SaleItem>> ListByFilterAsync(Guid? saleId, string? productName, CancellationToken cancellationToken = default);
}
