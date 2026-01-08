namespace CoupleCentsAPI.Models.DTOs;

public class ExpenseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool AffectsFamilyBudget { get; set; }
    public int? WhoItAffects { get; set; }
    public string? WhoItAffectsName { get; set; }
    public List<KeywordDto> Keywords { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
public class ExpenseSummaryDto
{
    public int Id { get; set; }
    public string ExpenseName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int TypeId { get; set; }
}
public class ExpenseTypeDto
{
    public int TypeId { get; set; }
    public string Name { get; set; } = string.Empty;
}