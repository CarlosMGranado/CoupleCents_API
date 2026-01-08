using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Infrastructure.Repositories;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Queries.GetAllExpenses;

public class GetAllExpensesHandler : IRequestHandler<GetAllExpensesQuery, BaseResponse<List<ExpenseSummaryDto>>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetAllExpensesHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<BaseResponse<List<ExpenseSummaryDto>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var expenses = await _expenseRepository.GetAllSummariesAsync();

            return BaseResponse<List<ExpenseSummaryDto>>.SuccessResult(expenses, $"Retrieved {expenses.Count} expenses successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<ExpenseSummaryDto>>.FailureResult($"Error retrieving expenses: {ex.Message}");
        }
    }
}