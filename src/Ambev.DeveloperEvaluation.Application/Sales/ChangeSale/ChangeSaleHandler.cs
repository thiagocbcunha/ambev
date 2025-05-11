using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Handler for processing ChangeSaleCommand requests.
/// </summary>
public class ChangeSaleHandler(ISaleRepository saleRepository, IUserRepository userRepository, IMapper mapper) 
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

        var seller = await userRepository.GetByIdAsync(command.SallerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Seller with ID {command.SallerId} not found.");

        var customer = await userRepository.GetByIdAsync(command.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {command.CustomerId} not found.");

        seller.ThrowIfIsInvalidSeller();
        customer.ThrowIfIsInvalidCustomer();

        sale.Update(
            command.SaleNumber,
            command.SaleDate,
            seller.Id,
            customer.Id,
            command.BranchId, 
            command.BranchName);

        await saleRepository.UpdateAsync(sale, cancellationToken);

        return mapper.Map<ChangeSaleResult>(sale);
    }
}
