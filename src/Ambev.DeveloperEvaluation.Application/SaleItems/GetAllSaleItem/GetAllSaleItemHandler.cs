using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetAllSaleItem;

/// <summary>
/// Handler for processing GetSaleCommand requests.
/// Initializes a new instance of GetSaleHandler.
/// </summary>
/// <param name="saleRepository">The sale repository.</param>
/// <param name="mapper">The AutoMapper instance.</param>
public class GetAllSaleItemHandler(ISaleRepository saleRepository, IMapper mapper) : IRequestHandler<GetAllSaleItemCommand, IEnumerable<GetSaleItemResult>>
{
    /// <summary>
    /// Handles the GetSaleCommand request.
    /// </summary>
    /// <param name="commad">The GetSale command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The details of the requested sale.</returns>
    public async Task<IEnumerable<GetSaleItemResult>> Handle(GetAllSaleItemCommand commad, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(commad.SaleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {commad.SaleId} not found.");

        return sale.Items.Select(mapper.Map<GetSaleItemResult>);
    }
}

