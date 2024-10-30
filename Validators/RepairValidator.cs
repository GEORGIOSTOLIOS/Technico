using Technico.Models;

namespace Technico.Validators;

using FluentValidation;

public class RepairValidator : AbstractValidator<Repair>
{
    public RepairValidator()
    {
        RuleFor(r => r.Type)
            .IsInEnum()
            .WithMessage("Invalid repair type specified.");
        
        RuleFor(r => r.DateTime)
            .NotEmpty()
            .WithMessage("Repair date and time must be specified.")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Repair date cannot be in the future.");
        
        RuleFor(r => r.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");
        
        RuleFor(r => r.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MaximumLength(200)
            .WithMessage("Address cannot exceed 200 characters.");
        
        RuleFor(r => r.Status)
            .IsInEnum()
            .WithMessage("Invalid status specified.");
        
        RuleFor(r => r.Cost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Cost cannot be negative.")
            .PrecisionScale(10, 2, true)
            .WithMessage("Cost must have up to 10 digits in total with 2 decimal places.");
    }
}
