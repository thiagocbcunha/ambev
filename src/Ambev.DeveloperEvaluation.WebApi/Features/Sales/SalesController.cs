using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.PatchSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales.
/// Initializes a new instance of the SaleController.
/// </summary>
/// <param name="mediator">The mediator instance for handling requests.</param>
[ApiController]
[Route("api/[controller]")]
public class SalesController(IMediator mediator) : BaseController
{
    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="command">The command containing sale details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the created sale.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResult>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResult>
        {
            Data = result,
            Success = true,
            Message = "Sale created successfully"
        });
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The details of the requested sale.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseWithData<GetUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSaleCommand(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to update.</param>
    /// <param name="command">The command containing updated sale details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the updated sale.</returns>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseWithData<GetUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] PatchSaleCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<PatchSaleResult>
        {
            Data = result,
            Success = true,
            Message = "Sale changed successfully"
        });
    }

    /// <summary>
    /// Deletes a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseWithData<GetUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSale(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteSaleCommand(id), cancellationToken);

        if(result.Success)
            return Ok("Sale deleted successfully");

        return BadRequest("Error deleting sale");
    }
}

