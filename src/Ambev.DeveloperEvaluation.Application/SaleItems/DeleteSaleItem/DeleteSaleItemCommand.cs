using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

/// <summary>
/// Command for deleting a sale.
/// </summary>
public record DeleteSaleItemCommand(Guid Id) : IRequest<DeleteSaleItemResponse>
{}
