using FluentValidation;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Validators;

public abstract class BaseTaskValidator<T> : AbstractValidator<T>
    where T : class, ITaskRequest
{
    protected BaseTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be at most 1000 characters.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.UtcNow).When(x => x.Deadline.HasValue)
            .WithMessage("Deadline date must be in the future.");
    }
}
