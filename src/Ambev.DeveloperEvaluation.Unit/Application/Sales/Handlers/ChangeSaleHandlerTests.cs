using Xunit;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers;

/// <summary>
/// Contains unit tests for the <see cref="ChangeSaleHandler"/> class.
/// </summary>
public class ChangeSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IUserRepository _userRepository;
    private readonly ChangeSaleHandler _handler;

    public ChangeSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new ChangeSaleHandler(_saleRepository, _userRepository, _mapper);
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given valid sale data When changing sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = ChangeSaleHandlerTestData.GenerateValidChangeSaleCommand();
        var sale = new Sale(command.SaleNumber, command.SaleDate, command.SallerId, command.CustomerId, command.BranchId, command.BranchName);
        var result = new ChangeSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            SallerId = sale.SallerId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.CustomerId, Status = UserStatus.Active });
        _userRepository.GetByIdAsync(command.SallerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.SallerId, Role = UserRole.Seller, Status = UserStatus.Active });
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<ChangeSaleResult>(sale).Returns(result);

        // When
        var actualResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(result);
        await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(s =>
            s.SaleNumber == command.SaleNumber &&
            s.SaleDate == command.SaleDate &&
            s.CustomerId == command.CustomerId &&
            s.BranchId == command.BranchId &&
            s.BranchName == command.BranchName), Arg.Any<CancellationToken>());
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given invalid sale ID When changing sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidSaleId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = ChangeSaleHandlerTestData.GenerateValidChangeSaleCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found.");
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given invalid customer ID When changing sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidCustomerId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = ChangeSaleHandlerTestData.GenerateValidChangeSaleCommand();
        var sale = new Sale(command.SaleNumber, command.SaleDate, command.SallerId, Guid.NewGuid(), command.BranchId, command.BranchName);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        
        _userRepository.GetByIdAsync(command.SallerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.SallerId, Role = UserRole.Seller, Status = UserStatus.Active });

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Customer with ID {command.CustomerId} not found.");
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given invalid seller ID When changing sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidSellerId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = ChangeSaleHandlerTestData.GenerateValidChangeSaleCommand();
        var sale = new Sale(command.SaleNumber, command.SaleDate, Guid.NewGuid(), command.CustomerId, command.BranchId, command.BranchName);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _userRepository.GetByIdAsync(command.SallerId, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Seller with ID {command.SallerId} not found.");
    }
}