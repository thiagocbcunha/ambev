using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for retrieving a sale by its unique identifier.
/// Initializes a new instance of GetSaleCommand.
/// </summary>
/// <param name="id">The ID of the sale to retrieve.</param>
public record GetSaleCommand(Guid Id) : IRequest<GetSaleResult>
{ }

