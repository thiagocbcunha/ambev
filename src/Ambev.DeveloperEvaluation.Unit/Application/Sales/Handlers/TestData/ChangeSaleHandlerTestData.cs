using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.PatchSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;

/// <summary>
/// Provides methods for generating test data for ChangeSaleCommand.
/// </summary>
public static class ChangeSaleHandlerTestData
{
    private static readonly Faker<PatchSaleCommand> changeSaleFaker = new Faker<PatchSaleCommand>()
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.SaleNumber, f => f.Random.Int(1, 1000))
        .RuleFor(s => s.SallerId, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchId, f => f.Random.String2(5, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName());

    /// <summary>
    /// Generates a valid ChangeSaleCommand.
    /// </summary>
    public static PatchSaleCommand GenerateValidChangeSaleCommand()
    {
        return changeSaleFaker.Generate();
    }
}

