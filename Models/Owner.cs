using System.ComponentModel.DataAnnotations;

namespace Technico.Models;

public class Owner
{   
    public int Id { get; set; }

    [Required(ErrorMessage = "VAT number is required")]
    [MinLength(1, ErrorMessage = "VAT number cannot be blank")]
    public required string VatNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public OwnerType Type { get; set; }

    public List<Property> Properties { get; set; } = [];

    public List<Repair> Repairs { get; set; } = [];

    
    public override string ToString()
    {
        return $"VAT: {VatNumber}, Name: {FirstName} {LastName}";
    }

    public void ChangeTo(Owner owner)
    {
        this.FirstName = owner.FirstName;
        this.LastName = owner.LastName;
        this.Properties = owner.Properties;
        this.Address = owner.Address;
        this.VatNumber = owner.VatNumber;
        this.Email = owner.Email;
        this.Password = owner.Password;
        this.Type = owner.Type;
        this.PhoneNumber = owner.PhoneNumber;
        this.Repairs = owner.Repairs;

        
    }
}