using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command for deleting a sale.
/// Initializes a new instance of DeleteSaleCommand.
/// </summary>
/// <param name="id">The ID of the sale to delete.</param>
public record DeleteSaleCommand(Guid Id) : IRequest<DeleteSaleResult>
{ }
