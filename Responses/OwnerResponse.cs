using Technico.Models;

namespace Technico.Responses;
//NOT ALL ELEMENTS ARE INCLUDED
public record OwnerResponse(string VatNumber, string FirstName, string LastName, string Address, 
       string PhoneNumber, string Email, List<Property> Properties, List<Repair> Repairs);