namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

internal record SalesItemCreated(CreateSaleItemResult Data)
{
    public Guid Id { get; } = Guid.NewGuid();
}
