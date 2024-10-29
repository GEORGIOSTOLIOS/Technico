using Technico.Models;

namespace Technico.Responses;
//NOT ALL ELEMENTS ARE INCLUDED
public record OwnerResponse(
    string VatNumber,
    string FirstName,
    string LastName,
    string Address,
    string PhoneNumber,
    string Email,
    List<Property> Properties,
    List<RepairDetail> Repairs)
{
    public override string ToString()
    {
        var repairInfo = string.Join(", ", Repairs.Select(r => r.ToString()));
        return $"OwnerResponse {{ VatNumber = {VatNumber}, Name = {FirstName} {LastName}, Address = {Address}, " +
               $"PhoneNumber = {PhoneNumber}, Email = {Email}, Repairs = [{repairInfo}] }}";
    }
}
       
       