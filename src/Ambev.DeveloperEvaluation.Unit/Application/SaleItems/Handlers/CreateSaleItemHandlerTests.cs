using Xunit;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.SaleItems.Handlers;

public class CreateSaleItemHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly CreateSaleItemHandler _handler;

    public CreateSaleItemHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _saleItemRepository = Substitute.For<ISaleItemRepository>();
        _handler = new CreateSaleItemHandler(_saleItemRepository, _mapper);
    }

    [Trait("SaleItems", "Handler")]
    [Fact(DisplayName = "Given valid sale item data When creating sale item Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = new CreateSaleItemCommand();
        var saleItem = new SaleItem();
        var expectedResult = new CreateSaleItemResult();

        _mapper.Map<SaleItem>(command).Returns(saleItem);
        _saleItemRepository.CreateAsync(saleItem, Arg.Any<CancellationToken>())
            .Returns(saleItem);
        _mapper.Map<CreateSaleItemResult>(saleItem).Returns(expectedResult);

        // When
        var actualResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
