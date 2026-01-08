using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand(
    string Name,
    int TypeId,
    bool AffectsFamilyBudget,
    int? WhoItAffects,
    List<string> Keywords
) : IRequest<BaseResponse<ExpenseDto>>;