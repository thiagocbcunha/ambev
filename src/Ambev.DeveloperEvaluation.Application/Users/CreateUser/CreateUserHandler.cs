using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Handler for processing CreateUserCommand requests
/// Initializes a new instance of CreateUserHandler
/// </summary>
/// <param name="userRepository">The user repository</param>
/// <param name="mapper">The AutoMapper instance</param>
/// <param name="validator">The validator for CreateUserCommand</param>
public class CreateUserHandler(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher) 
    : IRequestHandler<CreateUserCommand, CreateUserResult>
{

    /// <summary>
    /// Handles the CreateUserCommand request
    /// </summary>
    /// <param name="command">The CreateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        //var validator = new CreateUserCommandValidator();
        //var validationResult = await validator.ValidateAsync(command, cancellationToken);

        //if (!validationResult.IsValid)
        //    throw new ValidationException(validationResult.Errors);

        var existingUser = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {command.Email} already exists");

        var user = mapper.Map<User>(command);
        user.Password = passwordHasher.HashPassword(command.Password);

        var createdUser = await userRepository.CreateAsync(user, cancellationToken);
        var result = mapper.Map<CreateUserResult>(createdUser);
        return result;
    }
}
