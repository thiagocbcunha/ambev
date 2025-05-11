namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

internal record SalesCreated(CreateSaleResult Data)
{
    public Guid Id { get; } = Guid.NewGuid();
}
