using MediatR;
using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Domain.Services;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, BaseResponse<ExpenseDto>>
{
    private readonly IExpenseService _expenseService;

    public CreateExpenseHandler(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    public async Task<BaseResponse<ExpenseDto>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        // Normalize: treat 0 as null
        var normalizedWhoItAffects = request.WhoItAffects == 0 ? null : request.WhoItAffects;

        var createRequest = new CreateExpenseRequest
        {
            Name = request.Name,
            TypeId = request.TypeId,
            AffectsFamilyBudget = request.AffectsFamilyBudget,
            WhoItAffects = normalizedWhoItAffects,
            Keywords = request.Keywords ?? new List<string>()
        };

        return await _expenseService.CreateExpenseAsync(createRequest);
    }
}