using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Domain.Services;

public interface IExpenseService
{
    Task<BaseResponse<ExpenseDto>> CreateExpenseAsync(CreateExpenseRequest request);
    Task<BaseResponse<ExpenseDto>> GetExpenseAsync(int id);
}