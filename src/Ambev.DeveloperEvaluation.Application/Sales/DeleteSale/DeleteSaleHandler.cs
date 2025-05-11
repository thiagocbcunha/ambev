using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests.
/// Initializes a new instance of DeleteSaleHandler.
/// </summary>
/// <param name="saleRepository">The sale repository.</param>
public class DeleteSaleHandler(ISaleRepository saleRepository) : IRequestHandler<DeleteSaleCommand, DeleteSaleResult>
{
    /// <summary>
    /// Handles the DeleteSaleCommand request.
    /// </summary>
    /// <param name="command">The DeleteSale command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the delete operation.</returns>
    public async Task<DeleteSaleResult> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.Id} not found.");

        var success = await saleRepository.DeleteAsync(sale.Id, cancellationToken);
        if (!success)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        return new DeleteSaleResult { Success = true };
    }
}
