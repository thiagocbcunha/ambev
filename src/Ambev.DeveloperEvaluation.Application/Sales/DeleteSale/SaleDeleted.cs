namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

internal record SaleDeleted(DeleteSaleCommand Data)
{
    public Guid Id { get; } = Guid.NewGuid();
}
