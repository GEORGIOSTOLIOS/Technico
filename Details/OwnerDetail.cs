namespace Technico.Details;

public class OwnerDetail
{
    public required string VatNumber { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
    
    public override string ToString()
    {
        return $"OwnerDetail: VAT = {VatNumber}, Name = {Name}";
    }
}