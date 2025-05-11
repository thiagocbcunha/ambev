using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;

/// <summary>
/// Provides methods for generating test data for sales commands.
/// </summary>
public static class SalesHandlerTestData
{
    private static readonly Faker<CreateSaleCommand> createSaleFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => f.Random.Int(1, 1000))
        .RuleFor(s => s.SaleDate, f => f.Date.Past())
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.SallerId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchId, f => f.Random.String2(5, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => new List<CreateSaleItemDto>
        {
            new CreateSaleItemDto
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = f.Commerce.ProductName(),
                Quantity = f.Random.Int(1, 10),
                UnitPrice = f.Random.Decimal(1, 100)
            }
        });

    /// <summary>
    /// Generates a valid CreateSaleCommand.
    /// </summary>
    public static CreateSaleCommand GenerateValidCreateSaleCommand()
    {
        return createSaleFaker.Generate();
    }
}
