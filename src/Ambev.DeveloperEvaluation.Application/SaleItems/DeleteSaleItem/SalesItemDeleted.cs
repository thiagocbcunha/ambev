namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

internal record SalesItemDeleted(DeleteSaleItemCommand Data)
{
    public Guid Id { get; } = Guid.NewGuid();
}
