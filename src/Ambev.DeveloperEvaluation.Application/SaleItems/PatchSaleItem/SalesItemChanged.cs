namespace Ambev.DeveloperEvaluation.Application.SaleItems.PatchSaleItem;

internal record SalesItemChanged(PatchSaleItemResult Data)
{
    public Guid Id { get; } = Guid.NewGuid();
}
