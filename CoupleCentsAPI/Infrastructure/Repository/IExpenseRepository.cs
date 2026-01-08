using CoupleCentsAPI.Models.DTOs;

public interface IExpenseRepository
{
    Task<bool> ExpenseTypeExistsAsync(int typeId);
    Task<bool> UserExistsAsync(int userId);
    Task<int> CreateExpenseAsync(CreateExpenseRequest request);
    Task<ExpenseDto?> GetByIdWithDetailsAsync(int id);
    Task<ExpenseDto?> GetByIdAsync(int id);

    Task<List<ExpenseSummaryDto>> GetAllSummariesAsync();

    Task<List<ExpenseTypeDto>> GetAllAsync();
    Task<ExpenseTypeDto?> GetTypeByIdAsync(int id);
}