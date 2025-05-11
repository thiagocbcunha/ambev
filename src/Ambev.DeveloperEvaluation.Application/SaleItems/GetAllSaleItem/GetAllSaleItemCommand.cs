using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetAllSaleItem;

/// <summary>
/// Command for retrieving a sale by its unique identifier.
/// </summary>
public record GetAllSaleItemCommand(Guid SaleId) : IRequest<IEnumerable<GetSaleItemResult>>
{ }

