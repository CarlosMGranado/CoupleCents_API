using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Domain.Services;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Queries.GetExpense;

public class GetExpenseHandler : IRequestHandler<GetExpenseQuery, BaseResponse<ExpenseDto>>
{
    private readonly IExpenseService _expenseService;

    public GetExpenseHandler(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    public async Task<BaseResponse<ExpenseDto>> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
    {
        return await _expenseService.GetExpenseAsync(request.Id);
    }
}