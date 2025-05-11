using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.PatchSale;

/// <summary>
/// Handler for processing <see cref="PatchSaleCommand"/> requests.
/// </summary>
public class PatchSaleHandler(ISaleRepository saleRepository, IUserRepository userRepository, IEventBroker eventBroker, IMapper mapper)
    : IRequestHandler<PatchSaleCommand, PatchSaleResult>
{
    /// <summary>
    /// Handles the <see cref="PatchSaleCommand"/> request.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<PatchSaleResult> Handle(PatchSaleCommand command, CancellationToken cancellationToken)
    {
        // Somente para fins didáticos, poderíamos executar as três task, onde uma não depende
        // da outra, de forma simutânea, contudo o EF tem uma limitação. Ele não deixa isso acontecer.
        // Isso acontence para que o dbcontext não seja corrompido.
        //var saleTask = GetSaleAsync(command.Id, cancellationToken);
        //var sellerTask = GetSallerIdAsync(command.SallerId, cancellationToken);
        //var customerTask = GetCustomerIdAsync(command.CustomerId, cancellationToken);        
        // await Task.WhenAll(saleTask, sellerTask, customerTask);

        var sale = await GetSaleAsync(command.Id, cancellationToken);
        var seller = await GetSallerIdAsync(command.SallerId, cancellationToken);
        var customer = await GetCustomerIdAsync(command.CustomerId, cancellationToken);
        // Com isso temos que de fato executa cada task uma seguida da outra, mesmo que não sejam dependentes.

        sale.Patch(
            command.SaleNumber,
            seller,
            customer,
            command.BranchId,
            command.BranchName);

        await saleRepository.UpdateAsync(sale, cancellationToken);

        var result = mapper.Map<PatchSaleResult>(sale);

        await eventBroker.PublishAsync(new SaleChanged(result));

        return result;
    }

    private async Task<Sale> GetSaleAsync(Guid id, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {id} not found.");

        return sale;
    }

    /// <summary>
    /// Retrieves the customer ID based on the provided customer ID and sale customer ID.
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    private async Task<Guid?> GetCustomerIdAsync(Guid? customerId, CancellationToken cancellationToken)
    {
        if (customerId is null)
            return null;

        var customer = await userRepository.GetByIdAsync(customerId.Value, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        customer.ThrowIfIsInvalidCustomer();

        return customer.Id;
    }

    /// <summary>
    /// Retrieves the seller ID based on the provided seller ID and sale seller ID.
    /// </summary>
    /// <param name="sallerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    private async Task<Guid?> GetSallerIdAsync(Guid? sallerId, CancellationToken cancellationToken)
    {
        if (sallerId is null)
            return null;

        var seller = await userRepository.GetByIdAsync(sallerId.Value, cancellationToken)
            ?? throw new KeyNotFoundException($"Seller with ID {sallerId} not found.");

        seller.ThrowIfIsInvalidSeller();

        return seller.Id;
    }
}