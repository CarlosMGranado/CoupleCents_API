using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Queries.GetAllExpenses;

public record GetAllExpensesQuery : IRequest<BaseResponse<List<ExpenseSummaryDto>>>;