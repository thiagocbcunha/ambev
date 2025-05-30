using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.PatchSaleItem;

/// <summary>
/// Handler for processing ChangeSaleCommand requests.
/// </summary>
public class PatchSaleItemHandler(ISaleItemRepository saleItemRepository, IEventBroker eventBroker, IMapper mapper)
    : IRequestHandler<PatchSaleItemCommand, PatchSaleItemResult>
{
    /// <summary>
    /// Handles the ChangeSaleCommand request.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<PatchSaleItemResult> Handle(PatchSaleItemCommand command, CancellationToken cancellationToken)
    {
        var saleItem = await saleItemRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"SaleItem with ID {command.Id} not found.");

        saleItem.Patch(
            command.ProductId,
            command.ProductName, 
            command.Quantity, 
            command.UnitPrice);

        saleItem.ThrowIfIsInvalidSaleItem();

        await saleItemRepository.UpdateAsync(saleItem, cancellationToken);

        var result = mapper.Map<PatchSaleItemResult>(saleItem);

        await eventBroker.PublishAsync(new SalesItemChanged(result));

        return result;
    }
}
