using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.Cache;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
/// <param name="context"></param>
public class SaleRepository(DefaultContext context) : ISaleRepository
{
    /// <inheritdoc />
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await context.Set<Sale>().AddAsync(sale, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <inheritdoc />
    [CachePolicy(60)]
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Set<Sale>()
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    [CachePolicy(60)]
    public async Task<List<Sale>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<Sale>()
            .Include(s => s.Items)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    [CachePolicy(60)]
    public async Task<List<Sale>> ListByFilterAsync(DateTime? startDate, DateTime? endDate, string? customerName, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Sale>().AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.SaleDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.SaleDate <= endDate.Value);

        if (!string.IsNullOrEmpty(customerName))
            query = query.Where(s => s.CustomerName.Contains(customerName));

        return await query.Include(s => s.Items).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Sale?> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        context.Set<Sale>().Update(sale);
        await context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        context.Set<Sale>().Remove(sale);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<List<Sale>> ListByFilterAsync(Expression<Func<Sale, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Sale>().Where(filter).AsQueryable();
        return await query.Include(i => i.Items).ToListAsync(cancellationToken);
    }
}
