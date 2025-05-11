using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

/// <summary>
/// Handler for processing DeleteSaleCommand requests.
/// Initializes a new instance of DeleteSaleHandler.
/// </summary>
/// <param name="saleRepository">The sale repository.</param>
public class DeleteSaleItemHandler(ISaleItemRepository saleItemRepository) : IRequestHandler<DeleteSaleItemCommand, DeleteSaleItemResponse>
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

        return new DeleteSaleItemResponse { Success = true };
    }
}
