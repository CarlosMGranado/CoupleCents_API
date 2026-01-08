using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Infrastructure.Repositories;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Queries.GetAllExpenseTypes;

public class GetAllExpenseTypesHandler : IRequestHandler<GetAllExpenseTypesQuery, BaseResponse<List<ExpenseTypeDto>>>
{
    private readonly IExpenseRepository _expenseTypeRepository;

    public GetAllExpenseTypesHandler(IExpenseRepository expenseTypeRepository)
    {
        _expenseTypeRepository = expenseTypeRepository;
    }

    public async Task<BaseResponse<List<ExpenseTypeDto>>> Handle(GetAllExpenseTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var expenseTypes = await _expenseTypeRepository.GetAllAsync();

            return BaseResponse<List<ExpenseTypeDto>>.SuccessResult(expenseTypes, $"Retrieved {expenseTypes.Count} expense types successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<ExpenseTypeDto>>.FailureResult($"Error retrieving expense types: {ex.Message}");
        }
    }
}