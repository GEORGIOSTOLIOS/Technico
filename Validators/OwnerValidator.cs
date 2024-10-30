using Technico.Models;

namespace Technico.Validators;
using FluentValidation;
public class OwnerValidator: AbstractValidator<Owner>
{
    public OwnerValidator()
    {   
        RuleFor(o => o.Email).EmailAddress();
        RuleFor(owner => owner.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{3}-\d{3}-\d{4}$").WithMessage("Invalid phone number format. Expected format: 123-456-7890");
        
        RuleFor(o => o.FirstName).NotEmpty();
        RuleFor(o => o.LastName).NotEmpty();
        RuleFor(o => o.Type).NotEmpty();
        
        RuleFor(o => o.Password)
            .NotEmpty().WithMessage("Password is required.") 
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
    }
