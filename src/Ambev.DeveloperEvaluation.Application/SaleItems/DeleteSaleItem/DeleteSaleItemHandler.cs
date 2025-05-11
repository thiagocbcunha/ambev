using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

/// <summary>
/// Handler for processing DeleteSaleCommand requests.
/// Initializes a new instance of DeleteSaleHandler.
/// </summary>
/// <param name="saleRepository">The sale repository.</param>
/// <param name="eventBroker">The event broker.</param>
public class DeleteSaleItemHandler(ISaleItemRepository saleItemRepository, IEventBroker eventBroker) : IRequestHandler<DeleteSaleItemCommand, DeleteSaleItemResponse>
{
    /// <summary>
    /// Handles the DeleteSaleCommand request.
    /// </summary>
    /// <param name="command">The DeleteSale command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the delete operation.</returns>
    public async Task<DeleteSaleItemResponse> Handle(DeleteSaleItemCommand command, CancellationToken cancellationToken)
    {
        var saleItem = await saleItemRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"SaleItem with ID {command.Id} not found.");

        var success = await saleItemRepository.DeleteAsync(saleItem.Id, cancellationToken);

        if (!success)
            throw new Exception($"Failed to delete SaleItem with ID {command.Id}.");

        await eventBroker.PublishAsync(new SalesItemDeleted(command));

        return new DeleteSaleItemResponse { Success = true };
    }
}
