using Technico.Details;
using Technico.Models;

namespace Technico.Responses;

public record PropertyResponse(
    string IdentNum,
    string Address,
    int ConstructionYear,
    PropertyType PropertyType,
    List<OwnerDetail> Owners)
{
    public override string ToString()
    {
        var ownerDetails = string.Join(", ", Owners.Select(o => $"{o.Name} (VAT: {o.VatNumber})"));
        return $"PropertyResponse {{ IdentNum = {IdentNum}, Address = {Address}, ConstructionYear = {ConstructionYear}, PropertyType = {PropertyType}, Owners = [{ownerDetails}] }}";
    }
}
       