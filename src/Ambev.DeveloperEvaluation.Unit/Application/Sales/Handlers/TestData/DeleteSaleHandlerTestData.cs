using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;

/// <summary>
/// Provides methods for generating test data for DeleteSaleCommand.
/// </summary>
public static class DeleteSaleHandlerTestData
{
    private static readonly Faker<DeleteSaleCommand> deleteSaleFaker = new Faker<DeleteSaleCommand>()
        .CustomInstantiator(f => new DeleteSaleCommand(Guid.NewGuid()));

    /// <summary>
    /// Generates a valid DeleteSaleCommand.
    /// </summary>
    public static DeleteSaleCommand GenerateValidDeleteSaleCommand()
    {
        return deleteSaleFaker.Generate();
    }
}
