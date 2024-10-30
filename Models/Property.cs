namespace Technico.Models;

public class Property
{
    public int Id { get; set; }
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int YearOfConstruction { get; set; }
    public PropertyType Type { get; set; }
    public List<Owner>? Owners { get; set; } = [];
    
}