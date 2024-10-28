using Technico.Models;

namespace Technico.Responses;

public record PropertyResponse(string IdentNum, String Address, 
       int ConstructionYear, PropertyType PropertyType, List<Owner> Owners);