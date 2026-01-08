using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Queries.GetExpense;

public record GetExpenseQuery(int Id) : IRequest<BaseResponse<ExpenseDto>>;