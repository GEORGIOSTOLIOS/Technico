using CSharpFunctionalExtensions;
using Technico.Models;
using Technico.Responses;

namespace Technico.Iservices;

public interface IPropertyService
{ 
     Task<Result<PropertyResponse>> CreateProperty (Property property, List<string> ownersVatNumbers);
     Task<Result<PropertyResponse>> GetProperty (int id);
     Task<Result<PropertyResponse>> UpdateProperty (int oldPropertyId, Property newProperty);
     Task<Result> DeleteProperty (int propertyId);
     Task<Result> DeactivateProperty(int id);
}