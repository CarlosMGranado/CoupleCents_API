using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoupleCentsAPI.Features.Expenses.Commands.CreateExpense;
using CoupleCentsAPI.Features.Expenses.Queries.GetExpense;
using CoupleCentsAPI.Features.Expenses.Queries.GetAllExpenses;
using CoupleCentsAPI.Features.Expenses.Queries.GetAllExpenseTypes;

namespace CoupleCentsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense(CreateExpenseCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetExpense),
            new { id = result.Data!.Id },
            result
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpense(int id)
    {
        var query = new GetExpenseQuery(id);
        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var query = new GetAllExpensesQuery();
        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetAllExpenseTypes()
    {
        var query = new GetAllExpenseTypesQuery();
        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}