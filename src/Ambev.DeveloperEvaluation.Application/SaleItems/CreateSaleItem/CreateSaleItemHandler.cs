using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

/// <summary>
/// Handler for processing CreateSaleCommand requests.
/// </summary>
public class CreateSaleItemHandler(ISaleItemRepository saleRepository, IMapper mapper) 
    : IRequestHandler<CreateSaleItemCommand, CreateSaleItemResult>
{
    /// <summary>
    /// Handles the creation of a new sale.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateSaleItemResult> Handle(CreateSaleItemCommand command, CancellationToken cancellationToken)
    {
        var sale = mapper.Map<SaleItem>(command);
        var createdSale = await saleRepository.CreateAsync(sale, cancellationToken);

        return mapper.Map<CreateSaleItemResult>(createdSale);
    }
}
