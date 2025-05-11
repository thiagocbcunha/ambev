using Xunit;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Unit.Application.SaleItems.Handlers;

public class DeleteSaleItemHandlerTests
{
    private readonly IEventBroker _eventBroker;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly DeleteSaleItemHandler _handler;

    public DeleteSaleItemHandlerTests()
    {
        _eventBroker = Substitute.For<IEventBroker>();
        _saleItemRepository = Substitute.For<ISaleItemRepository>();
        _handler = new DeleteSaleItemHandler(_saleItemRepository, _eventBroker);
    }

    [Trait("SaleItems", "Handler")]
    [Fact(DisplayName = "Given valid sale item ID When deleting sale item Then returns success response")]
    public async Task Handle_ValidSaleItemId_ReturnsSuccessResponse()
    {
        // Given
        var command = new DeleteSaleItemCommand(Guid.NewGuid());
        var saleItem = new SaleItem();

        _saleItemRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(saleItem);
        _saleItemRepository.DeleteAsync(saleItem.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var actualResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        actualResult.Should().NotBeNull();
        actualResult.Success.Should().BeTrue();
    }

    [Trait("SaleItems", "Handler")]
    [Fact(DisplayName = "Given invalid sale item ID When deleting sale item Then throws KeyNotFoundException")]
    public async Task Handle_InvalidSaleItemId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = new DeleteSaleItemCommand(Guid.NewGuid());

        _saleItemRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((SaleItem?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"SaleItem with ID {command.Id} not found.");
    }
}
