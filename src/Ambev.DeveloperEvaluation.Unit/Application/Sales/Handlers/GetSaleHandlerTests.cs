using Xunit;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given valid sale ID When retrieving sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidGetSaleCommand();
        var sale = new Sale(999, Guid.NewGuid(), Guid.NewGuid(), "BR001", "Main Branch");

        var result = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerName = "John Doe",
            BranchName = sale.BranchName,
            TotalAmount = 100.0m,
            Items = new List<GetSaleItemDto>()
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // When
        var response = await _handler.Handle(command, CancellationToken.None);

        // Then
        response.Should().NotBeNull();
        response.Id.Should().Be(sale.Id);
        response.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given invalid sale ID When retrieving sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidRequest_ThrowsKeyNotFoundException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidGetSaleCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found.");
    }
}

