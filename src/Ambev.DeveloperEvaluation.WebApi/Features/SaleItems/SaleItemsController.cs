using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetAllSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.PatchSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems;

/// <summary>
/// Controller for managing sales.
/// Initializes a new instance of the SaleController.
/// </summary>
/// <param name="mediator">The mediator instance for handling requests.</param>
[ApiController]
[Route("api/[controller]")]
public class SalesItemsController(IMediator mediator) : BaseController
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
    public async Task<IActionResult> CreateSaleItem([FromBody] CreateSaleItemCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleItemResult>
        {
            Data = result,
            Success = true,
            Message = "Sale Item created successfully"
        });
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The details of the requested sale.</returns>
    [HttpGet("all-by-sale/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetSaleItemResult>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSaleItem([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllSaleItemCommand(id), cancellationToken);
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
    public async Task<IActionResult> UpdateSaleItem([FromRoute] Guid id, [FromBody] PatchSaleItemCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<PatchSaleItemResult>
        {
            Data = result,
            Success = true,
            Message = "Sale Item changed successfully"
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
    public async Task<IActionResult> DeleteSaleItem(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteSaleItemCommand(id), cancellationToken);

        if(result.Success)
            return Ok("Sale Item deleted successfully");

        return BadRequest("Error deleting sale");
    }
}

