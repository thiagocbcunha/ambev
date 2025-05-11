using Xunit;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.Handlers.TestData;
using Ambev.DeveloperEvaluation.Application.Sales.PatchSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handler.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IUserRepository _userRepository;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new CreateSaleHandler(_saleRepository, _userRepository, _mapper);
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = SalesHandlerTestData.GenerateValidCreateSaleCommand();
        var sale = new Sale(command.SaleNumber, command.SallerId, command.CustomerId, command.BranchId, command.BranchName);

        var result = new CreateSaleResult
        {
            Id = sale.Id,
            SaleDate = sale.SaleDate,
            SaleNumber = sale.SaleNumber,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount
        };

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.CustomerId, Status = UserStatus.Active });

        _userRepository.GetByIdAsync(command.SallerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.SallerId, Role = UserRole.Seller, Status = UserStatus.Active });

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var createUserResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createUserResult.Should().NotBeNull();
        createUserResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given invalid customer ID When creating sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidCustomerId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = SalesHandlerTestData.GenerateValidCreateSaleCommand();

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
    [Fact(DisplayName = "Given invalid seller ID When creating sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidSellerId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = SalesHandlerTestData.GenerateValidCreateSaleCommand();

        _userRepository.GetByIdAsync(command.SallerId, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Seller with ID {command.SallerId} not found.");
    }

    [Trait("Sales", "Handler")]
    [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
    public async Task Handle_ValidRequest_MapsCommandToSale()
    {
        // Given
        var command = SalesHandlerTestData.GenerateValidCreateSaleCommand();
        var sale = new Sale(command.SaleNumber, command.SallerId, command.CustomerId, command.BranchId, command.BranchName);

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.CustomerId, Status = UserStatus.Active });

        _userRepository.GetByIdAsync(command.SallerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.SallerId, Role = UserRole.Seller, Status = UserStatus.Active });

        _mapper.Map<Sale>(command).Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
            c.SaleNumber == command.SaleNumber &&
            c.SaleDate == command.SaleDate &&
            c.CustomerId == command.CustomerId &&
            c.BranchId == command.BranchId &&
            c.BranchName == command.BranchName));
    }
}