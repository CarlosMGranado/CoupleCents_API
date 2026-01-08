using Dapper;
using CoupleCentsAPI.Infrastructure.Data;
using CoupleCentsAPI.Models.DTOs;
using System.Data;

namespace CoupleCentsAPI.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ExpenseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExpenseTypeExistsAsync(int typeId)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        const string sql = "SELECT COUNT(1) FROM ExpenseType WHERE Id = @TypeId";
        var count = await connection.QuerySingleAsync<int>(sql, new { TypeId = typeId });
        return count > 0;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        const string sql = "SELECT COUNT(1) FROM User WHERE Id = @UserId";
        var count = await connection.QuerySingleAsync<int>(sql, new { UserId = userId });
        return count > 0;
    }

    public async Task<int> CreateExpenseAsync(CreateExpenseRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // Insert expense
            const string insertExpenseSql = @"
                INSERT INTO Expense (Name, TypeId, AffectsFamilyBudget, WhoItAffects, CreatedAt)
                VALUES (@Name, @TypeId, @AffectsFamilyBudget, @WhoItAffects, @CreatedAt);
                SELECT LAST_INSERT_ID();";

            var expenseId = await connection.QuerySingleAsync<int>(insertExpenseSql, new
            {
                request.Name,
                request.TypeId,
                request.AffectsFamilyBudget,
                request.WhoItAffects,
                CreatedAt = DateTime.UtcNow
            }, transaction);

            // Insert keywords if provided
            if (request.Keywords?.Any() == true)
            {
                const string insertKeywordsSql = @"
                    INSERT INTO Keyword (Name, ExpenseId, CreatedAt)
                    VALUES (@Name, @ExpenseId, @CreatedAt)";

                var keywordParams = request.Keywords
                    .Where(k => !string.IsNullOrWhiteSpace(k))
                    .Select(keyword => new
                    {
                        Name = keyword.Trim(),
                        ExpenseId = expenseId,
                        CreatedAt = DateTime.UtcNow
                    });

                if (keywordParams.Any())
                {
                    await connection.ExecuteAsync(insertKeywordsSql, keywordParams, transaction);
                }
            }

            transaction.Commit();
            return expenseId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        const string sql = @"
            SELECT 
                Id,
                Name,
                TypeId,
                AffectsFamilyBudget,
                WhoItAffects,
                Icon,
                CreatedAt
            FROM Expense 
            WHERE Id = @Id";

        return await connection.QuerySingleOrDefaultAsync<ExpenseDto>(sql, new { Id = id });
    }

    public async Task<ExpenseDto?> GetByIdWithDetailsAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        const string expenseSql = @"
            SELECT 
                e.Id,
                e.Name,
                e.TypeId,
                et.Name as TypeName,
                e.Icon,
                e.AffectsFamilyBudget,
                e.WhoItAffects,
                u.Name as WhoItAffectsName,
                e.CreatedAt
            FROM Expense e
            INNER JOIN ExpenseType et ON e.TypeId = et.Id
            LEFT JOIN User u ON e.WhoItAffects = u.Id
            WHERE e.Id = @Id";

        var expense = await connection.QuerySingleOrDefaultAsync<ExpenseDto>(expenseSql, new { Id = id });

        if (expense != null)
        {
            // Get keywords
            const string keywordsSql = @"
                SELECT Id, Name
                FROM Keyword
                WHERE ExpenseId = @ExpenseId
                ORDER BY Name";

            var keywords = await connection.QueryAsync<KeywordDto>(keywordsSql, new { ExpenseId = id });
            expense.Keywords = keywords.ToList();
        }

        return expense;
    }

    public async Task<List<ExpenseSummaryDto>> GetAllSummariesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        const string sql = @"
            SELECT 
                Id,
                Name as ExpenseName,
                Icon,
                TypeId
            FROM Expense 
            ORDER BY Name";

        var results = await connection.QueryAsync<ExpenseSummaryDto>(sql);
        return results.ToList();
    }

    public async Task<List<ExpenseTypeDto>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        const string sql = @"
            SELECT 
                Id as TypeId,
                Name
            FROM ExpenseType 
            ORDER BY Name";

        var results = await connection.QueryAsync<ExpenseTypeDto>(sql);
        return results.ToList();
    }

    public async Task<ExpenseTypeDto?> GetTypeByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        const string sql = @"
            SELECT 
                Id as TypeId,
                Name
            FROM ExpenseType 
            WHERE Id = @Id";

        return await connection.QuerySingleOrDefaultAsync<ExpenseTypeDto>(sql, new { Id = id });
    }
}
