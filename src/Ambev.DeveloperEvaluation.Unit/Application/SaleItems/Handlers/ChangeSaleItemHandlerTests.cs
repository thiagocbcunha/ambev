using Xunit;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.SaleItems.PatchSaleItem;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Unit.Application.SaleItems.Handlers;

public class ChangeSaleItemHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IEventBroker _eventBroker;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly PatchSaleItemHandler _handler;

    public ChangeSaleItemHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _eventBroker = Substitute.For<IEventBroker>();
        _saleItemRepository = Substitute.For<ISaleItemRepository>();
        _handler = new PatchSaleItemHandler(_saleItemRepository, _eventBroker, _mapper);
    }

    [Trait("SaleItems", "Handler")]
    [Fact(DisplayName = "Given valid sale item data When changing sale item Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = new PatchSaleItemCommand { Id = Guid.NewGuid(), ProductName = "Updated Product", Quantity = 10, UnitPrice = 20.0m };
        var saleItem = new SaleItem();
        var expectedResult = new PatchSaleItemResult();

        _saleItemRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(saleItem);
        _saleItemRepository.UpdateAsync(saleItem, Arg.Any<CancellationToken>())
            .Returns(saleItem);
        _mapper.Map<PatchSaleItemResult>(saleItem).Returns(expectedResult);

        // When
        var actualResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Trait("SaleItems", "Handler")]
    [Fact(DisplayName = "Given invalid sale item ID When changing sale item Then throws KeyNotFoundException")]
    public async Task Handle_InvalidSaleItemId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = new PatchSaleItemCommand { Id = Guid.NewGuid() };

        _saleItemRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((SaleItem?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"SaleItem with ID {command.Id} not found.");
    }
}
