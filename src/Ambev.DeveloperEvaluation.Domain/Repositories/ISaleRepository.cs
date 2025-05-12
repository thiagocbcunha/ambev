using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository : IRepository<Sale>
{
    /// <summary>
    /// Retrieves a list of sales filtered by date range and customer name.
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="custormerEmail"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Sale>> ListByFilterAsync(DateTime? startDate, DateTime? endDate, string? custormerEmail, CancellationToken cancellationToken = default);
}
