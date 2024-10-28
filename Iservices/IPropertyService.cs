using CSharpFunctionalExtensions;
using Technico.Models;
using Technico.Responses;

namespace Technico.Iservices;

public interface IPropertyService
{
    public Task<Result<PropertyResponse>> CreateProperty (Property property, List<string> ownersVatNumbers);
    
    public Task<Result<PropertyResponse>> GetProperty (int id);
    
    public Task<Result<PropertyResponse>> UpdateProperty (Property oldProperty, Property newProperty);
    
    public Task<Result> DeleteProperty (Property property);
}