using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests.
/// </summary>
public class CreateSaleHandler(ISaleRepository saleRepository, IUserRepository userRepository, IMapper mapper) 
    : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    /// <summary>
    /// Handles the creation of a new sale.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var seller = await userRepository.GetByIdAsync(command.SallerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Seller with ID {command.SallerId} not found.");

        var customer = await userRepository.GetByIdAsync(command.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {command.CustomerId} not found.");

        seller.ThrowIfIsInvalidSeller();
        customer.ThrowIfIsInvalidCustomer();

        var sale = mapper.Map<Sale>(command);
        var createdSale = await saleRepository.CreateAsync(sale, cancellationToken);

        return mapper.Map<CreateSaleResult>(createdSale);
    }
}
