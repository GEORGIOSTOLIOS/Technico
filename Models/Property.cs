namespace Technico.Models;

public class Property
{
    public int Id { get; set; }

    public string IdentificationNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
    public int YearOfConstruction { get; set; }
    
    public PropertyType Type { get; set; }
    public List<Owner> Owners { get; set; } = [];
    
    public override string ToString()
    {
        return $"E9: {IdentificationNumber}, Address: {Address} ";
    }

    public void ChangeTo(Property property)
    {
        this.IdentificationNumber = property.IdentificationNumber;
        this.Address = property.Address;
        this.YearOfConstruction = property.YearOfConstruction;
        this.Type = property.Type;
        this.Owners = property.Owners;
    }
}