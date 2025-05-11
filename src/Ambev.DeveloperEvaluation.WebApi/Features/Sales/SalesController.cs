using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the SaleController.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="command">The command containing sale details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the created sale.</returns>
    [HttpPost]
    public async Task<ActionResult<CreateSaleResult>> CreateSale([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetSale), new { id = result.Id }, result);
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The details of the requested sale.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetSaleResult>> GetSale(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSaleCommand(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to update.</param>
    /// <param name="command">The command containing updated sale details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the updated sale.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ChangeSaleResult>> UpdateSale(Guid id, [FromBody] ChangeSaleCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("The sale ID in the URL does not match the ID in the request body.");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<DeleteSaleResponse>> DeleteSale(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteSaleCommand(id), cancellationToken);
        return Ok(result);
    }
}

