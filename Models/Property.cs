using System.ComponentModel.DataAnnotations;

namespace Technico.Models;

public class Property
{
    public int Id { get; set; }
    [Required]
    [MaxLength(20)] 
    public string IdentificationNumber { get; set; } = string.Empty;

    [MaxLength(200)] 
    public string Address { get; set; } = string.Empty;
    public int YearOfConstruction { get; set; }
    public PropertyType Type { get; set; }
    public List<Owner>? Owners { get; set; } = [];
    
}