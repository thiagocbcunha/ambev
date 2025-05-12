namespace Ambev.DeveloperEvaluation.Application.Sales.PatchSale;

internal record SaleChanged(PatchSaleResult Data)
{
    public Guid Id { get; } = Guid.NewGuid();
}
