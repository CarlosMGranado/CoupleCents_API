using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Queries.GetAllExpenseTypes;

public record GetAllExpenseTypesQuery : IRequest<BaseResponse<List<ExpenseTypeDto>>>;