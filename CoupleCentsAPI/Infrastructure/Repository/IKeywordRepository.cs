using CoupleCentsAPI.Models.DTOs;

namespace CoupleCentsAPI.Infrastructure.Repositories;

public interface IKeywordRepository
{
    Task CreateKeywordsAsync(int expenseId, List<string> keywords);
    Task<List<KeywordDto>> GetByExpenseIdAsync(int expenseId);
}
