using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the User entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
public class UserTests
{
    /// <summary>
    /// Tests that when a suspended user is activated, their status changes to Active.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "User status should change to Active when activated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Suspended;

        // Act
        user.Activate();

        // Assert
        Assert.Equal(UserStatus.Active, user.Status);
    }

    /// <summary>
    /// Tests that when an active user is suspended, their status changes to Suspended.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "User status should change to Suspended when suspended")]
    public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Active;

        // Act
        user.Suspend();

        // Assert
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    /// <summary>
    /// Tests that validation passes when all user properties are valid.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "Validation should pass for valid user data")]
    public void Given_ValidUserData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();

        // Act
        var result = user.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when user properties are invalid.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "Validation should fail for invalid user data")]
    public void Given_InvalidUserData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var user = new User
        {
            Username = "", // Invalid: empty
            Password = UserTestData.GenerateInvalidPassword(), // Invalid: doesn't meet password requirements
            Email = UserTestData.GenerateInvalidEmail(), // Invalid: not a valid email
            Phone = UserTestData.GenerateInvalidPhone(), // Invalid: doesn't match pattern
            Status = UserStatus.Unknown, // Invalid: cannot be Unknown
            Role = UserRole.None // Invalid: cannot be None
        };

        // Act
        var result = user.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that an invalid seller throws an exception.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "Given invalid seller When validating Then throws exception")]
    public void ThrowIfIsInvalidSeller_InvalidSeller_ThrowsException()
    {
        // Given
        var user = new User
        {
            Role = UserRole.Customer,
            Status = UserStatus.Inactive
        };

        // When
        Action act = () => user.ThrowIfIsInvalidSeller();

        // Then
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User is not a seller.");
    }

    /// <summary>
    /// Tests that a valid seller does not throw an exception.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "Given valid seller When validating Then does not throw exception")]
    public void ThrowIfIsInvalidSeller_ValidSeller_DoesNotThrowException()
    {
        // Given
        var user = new User
        {
            Role = UserRole.Seller,
            Status = UserStatus.Active
        };

        // When
        Action act = () => user.ThrowIfIsInvalidSeller();

        // Then
        act.Should().NotThrow();
    }

    /// <summary>
    /// Tests that an invalid customer throws an exception.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "Given invalid customer When validating Then throws exception")]    
    public void ThrowIfIsInvalidCustomer_InvalidCustomer_ThrowsException()
    {
        // Given
        var user = new User
        {
            Role = UserRole.Seller,
            Status = UserStatus.Inactive
        };

        // When
        Action act = () => user.ThrowIfIsInvalidCustomer();

        // Then
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User is not active.");
    }

    /// <summary>
    /// Tests that a valid customer does not throw an exception.
    /// </summary>
    [Trait("User", "Entity")]
    [Fact(DisplayName = "Given valid customer When validating Then does not throw exception")]
    public void ThrowIfIsInvalidCustomer_ValidCustomer_DoesNotThrowException()
    {
        // Given
        var user = new User
        {
            Role = UserRole.Customer,
            Status = UserStatus.Active
        };

        // When
        Action act = () => user.ThrowIfIsInvalidCustomer();

        // Then
        act.Should().NotThrow();
    }
}
