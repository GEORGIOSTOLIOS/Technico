using System.ComponentModel.DataAnnotations;

namespace Technico.Models;

public class Owner
{   
    public int Id { get; set; }

    [Required]
    [MaxLength(20)] 
    public string VatNumber { get; set; } = string.Empty;

    [MaxLength(50)] 
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50)] 
    public string LastName { get; set; } = string.Empty;

    [MaxLength(200)] 
    public string Address { get; set; } = string.Empty;

    [MaxLength(20)] 
    public string PhoneNumber { get; set; } = string.Empty;
    
    [MaxLength(255)] 
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(100)] 
    public string Password { get; set; } = string.Empty;
    
    public OwnerType Type { get; set; }

    public List<Property>? Properties { get; set; }

    public List<Repair> Repairs { get; set; } = new();
}