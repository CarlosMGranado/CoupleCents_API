namespace CoupleCentsAPI.Models.DTOs;

public class CreateExpenseRequest
{
    public string Name { get; set; } = string.Empty;
    public int TypeId { get; set; }
    public bool AffectsFamilyBudget { get; set; }
    public int? WhoItAffects { get; set; }
    public List<string> Keywords { get; set; } = new();
}