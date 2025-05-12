using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;

/// <summary>
/// Provides methods for generating test data for GetSaleCommand.
/// </summary>
public static class GetSaleHandlerTestData
{
    private static readonly Faker<GetSaleCommand> getSaleFaker = new Faker<GetSaleCommand>()
        .CustomInstantiator(f => new GetSaleCommand(Guid.NewGuid()));

    /// <summary>
    /// Generates a valid GetSaleCommand.
    /// </summary>
    public static GetSaleCommand GenerateValidGetSaleCommand()
    {
        return getSaleFaker.Generate();
    }
}

