using Xunit;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly DeleteSaleHandler _handler;

    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleHandler(_saleRepository);
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given valid sale ID When deleting sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = DeleteSaleHandlerTestData.GenerateValidDeleteSaleCommand();

        var sale = new Sale(999, Guid.NewGuid(), Guid.NewGuid(), "BR001", "Main Branch");

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.DeleteAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        await _saleRepository.Received(1).DeleteAsync(sale.Id, Arg.Any<CancellationToken>());
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given invalid sale ID When deleting sale Then returns failure response")]
    public async Task Handle_InvalidRequest_ReturnsFailureResponse()
    {
        // Given
        var command = DeleteSaleHandlerTestData.GenerateValidDeleteSaleCommand();
        var sale = new Sale(999, Guid.NewGuid(), Guid.NewGuid(), "BR001", "Main Branch");

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.DeleteAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }
}
