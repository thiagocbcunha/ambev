using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.Cache;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleItemRepository using Entity Framework Core
/// </summary>
/// <param name="context"></param>
public class SaleItemRepository(DefaultContext context) : ISaleItemRepository
{
    /// <inheritdoc />
    public async Task<SaleItem> CreateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
    {
        await context.Set<SaleItem>().AddAsync(saleItem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return saleItem;
    }

    /// <inheritdoc />
    [CachePolicy(60)]
    public async Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Set<SaleItem>().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    [CachePolicy(60)]
    public async Task<List<SaleItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<SaleItem>().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    [CachePolicy(60)]
    public async Task<List<SaleItem>> ListByFilterAsync(Guid? saleId, string? productName, CancellationToken cancellationToken = default)
    {
        var query = context.Set<SaleItem>().AsQueryable();

        if (saleId.HasValue)
            query = query.Where(i => i.SaleId == saleId.Value);

        if (!string.IsNullOrEmpty(productName))
            query = query.Where(i => i.ProductName.Contains(productName));

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<SaleItem?> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
    {
        context.Set<SaleItem>().Update(saleItem);
        await context.SaveChangesAsync(cancellationToken);
        return saleItem;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var saleItem = await GetByIdAsync(id, cancellationToken);
        if (saleItem == null)
            return false;

        context.Set<SaleItem>().Remove(saleItem);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<List<SaleItem>> ListByFilterAsync(Expression<Func<SaleItem, bool>> filter, CancellationToken cancellationToken = default)
    {
        var query = context.Set<SaleItem>().Where(filter).AsQueryable();
        return await query.ToListAsync(cancellationToken);
    }
}
