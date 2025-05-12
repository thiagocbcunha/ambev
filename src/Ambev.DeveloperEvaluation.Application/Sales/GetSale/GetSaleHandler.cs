using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommand requests.
/// Initializes a new instance of GetSaleHandler.
/// </summary>
/// <param name="saleRepository">The sale repository.</param>
/// <param name="mapper">The AutoMapper instance.</param>
public class GetSaleHandler(ISaleRepository saleRepository, IMapper mapper) : IRequestHandler<GetSaleCommand, GetSaleResult>
{
    /// <summary>
    /// Handles the GetSaleCommand request.
    /// </summary>
    /// <param name="command">The GetSale command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The details of the requested sale.</returns>
    public async Task<GetSaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.Id} not found.");

        return mapper.Map<GetSaleResult>(sale);
    }
}

