using CoupleCentsAPI.Common.Responses;
using CoupleCentsAPI.Domain.Services;
using CoupleCentsAPI.Infrastructure.Repositories;
using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Domain.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<BaseResponse<ExpenseDto>> CreateExpenseAsync(CreateExpenseRequest request)
    {
        try
        {
            // Business validation
            var validationResult = await ValidateCreateExpenseAsync(request);
            if (!validationResult.IsSuccess)
            {
                return BaseResponse<ExpenseDto>.FailureResult(validationResult.Message);
            }

            // Create expense (repository handles the transaction)
            var expenseId = await _expenseRepository.CreateExpenseAsync(request);

            // Get the created expense
            var createdExpense = await _expenseRepository.GetByIdWithDetailsAsync(expenseId);

            if (createdExpense == null)
            {
                return BaseResponse<ExpenseDto>.FailureResult("Expense was created but could not be retrieved");
            }

            return BaseResponse<ExpenseDto>.SuccessResult(createdExpense, "Expense created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<ExpenseDto>.FailureResult($"Error creating expense: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ExpenseDto>> GetExpenseAsync(int id)
    {
        try
        {
            var expense = await _expenseRepository.GetByIdWithDetailsAsync(id);

            if (expense == null)
            {
                return BaseResponse<ExpenseDto>.FailureResult($"Expense with ID {id} not found");
            }

            return BaseResponse<ExpenseDto>.SuccessResult(expense, "Expense retrieved successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<ExpenseDto>.FailureResult($"Error retrieving expense: {ex.Message}");
        }
    }

    private async Task<(bool IsSuccess, string Message)> ValidateCreateExpenseAsync(CreateExpenseRequest request)
    {
        // Validate expense type exists
        var typeExists = await _expenseRepository.ExpenseTypeExistsAsync(request.TypeId);
        if (!typeExists)
        {
            return (false, "Invalid expense type ID");
        }

        // Validate user exists if provided
        if (request.WhoItAffects.HasValue)
        {
            var userExists = await _expenseRepository.UserExistsAsync(request.WhoItAffects.Value);
            if (!userExists)
            {
                return (false, "Invalid user ID");
            }
        }

        return (true, string.Empty);
    }
}