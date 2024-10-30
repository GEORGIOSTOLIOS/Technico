using System.ComponentModel.DataAnnotations;

namespace Technico.Models;

public class Owner
{   
    public int Id { get; set; }
    public required string VatNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public OwnerType Type { get; set; }

    public List<Property>? Properties { get; set; }

    public List<Repair> Repairs { get; set; } = [];
    
}