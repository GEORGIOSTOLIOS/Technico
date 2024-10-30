using Technico.Models;

namespace Technico.Validators;
using FluentValidation;

public class PropertyValidator: AbstractValidator<Property>
{
    public PropertyValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MinimumLength(5).WithMessage("Address must be at least 5 characters long.")
            .MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");
        
        RuleFor(x => x.YearOfConstruction)
            .InclusiveBetween(1800, DateTime.Now.Year)
            .WithMessage("Year of construction must be between 1800 and the current year.");

       
        RuleFor(p => p.Type)
            .IsInEnum()
            .WithMessage("Property type must be a valid enum value.");
        
    }
    }
