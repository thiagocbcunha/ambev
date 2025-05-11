using Xunit;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetAllSaleItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.SaleItems.Handlers;

public class GetAllSaleItemHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly GetAllSaleItemHandler _handler;

    public GetAllSaleItemHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new GetAllSaleItemHandler(_saleRepository, _mapper);
    }

    [Trait("SaleItems", "Handler")]
    [Fact(DisplayName = "Given valid sale ID When retrieving sale items Then returns mapped results")]
    public async Task Handle_ValidSaleId_ReturnsMappedResults()
    {
        // Given
        var command = new GetAllSaleItemCommand (Guid.NewGuid());
        var sale = new Sale();

        sale.AddItem(Guid.NewGuid().ToString(), "Test Product", 1, 10.0m);
        var expectedResult = new List<GetSaleItemResult> { new() };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<GetSaleItemResult>(Arg.Any<SaleItem>())
            .Returns(expectedResult.First());

        // When
        var actualResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Trait("SaleItems", "Handler")]
    [Fact(DisplayName = "Given invalid sale ID When retrieving sale items Then throws KeyNotFoundException")]
    public async Task Handle_InvalidSaleId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = new GetAllSaleItemCommand (Guid.NewGuid());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.SaleId} not found.");
    }
}
