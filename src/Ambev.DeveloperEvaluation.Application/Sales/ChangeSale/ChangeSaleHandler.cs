using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Handler for processing ChangeSaleCommand requests.
/// </summary>
public class ChangeSaleHandler(ISaleRepository saleRepository, IMapper mapper) 
    : IRequestHandler<ChangeSaleCommand, ChangeSaleResult>
{
    /// <summary>
    /// Handles the ChangeSaleCommand request.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<ChangeSaleResult> Handle(ChangeSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.Id} not found.");

        // Update sale properties
        sale.UpdateDetails(command.SaleNumber, command.SaleDate, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);

        // Update sale items
        sale.ClearItems();
        foreach (var item in command.Items)
        {
            sale.AddItem(Guid.Parse(item.ProductId), item.ProductName, item.Quantity, item.UnitPrice, 0);
        }

        await saleRepository.UpdateAsync(sale, cancellationToken);
        return mapper.Map<ChangeSaleResult>(sale);
    }
}
