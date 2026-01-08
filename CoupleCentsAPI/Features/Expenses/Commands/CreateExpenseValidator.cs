using FluentValidation;

namespace CoupleCentsAPI.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Expense name is required")
            .MaximumLength(200)
            .WithMessage("Expense name cannot exceed 200 characters");

        RuleFor(x => x.TypeId)
            .GreaterThan(0)
            .WithMessage("Valid expense type is required");

        // Updated validation - treat 0 as null (no user specified)
        RuleFor(x => x.WhoItAffects)
            .GreaterThan(0)
            .When(x => x.WhoItAffects.HasValue && x.WhoItAffects.Value > 0)
            .WithMessage("Valid user ID is required when specified");

        RuleFor(x => x.Keywords)
            .Must(keywords => keywords == null || keywords.All(k => !string.IsNullOrWhiteSpace(k)))
            .WithMessage("Keywords cannot be empty or whitespace");

        RuleForEach(x => x.Keywords)
            .MaximumLength(100)
            .WithMessage("Each keyword cannot exceed 100 characters")
            .When(x => x.Keywords != null);
    }
}