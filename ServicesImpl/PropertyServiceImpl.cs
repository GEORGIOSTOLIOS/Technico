using CSharpFunctionalExtensions;
using Technico.IRepositories;
using Technico.Iservices;
using Technico.Models;
using Technico.Responses;

namespace Technico.ServicesImpl;

public class PropertyServiceImpl: IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;

    public PropertyServiceImpl(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }
    public async Task<Result<PropertyResponse>> CreateProperty(Property property, List<string> ownersVatNumbers)
    {
        var propertyCreated = await _propertyRepository.CreateProperty(property, ownersVatNumbers);
        if (!propertyCreated)
        {
            return Result.Failure<PropertyResponse>("Owners not found");
        }

        var propertyResponse = MapToPropertyResponse(property);

        return Result.Success(propertyResponse);
    }

    public async Task<Result<PropertyResponse>> GetProperty(int id)
    {
        var property = await _propertyRepository.GetProperty(id);
        if (property == null)
        {
            return Result.Failure<PropertyResponse>("Property Not Found");
        }
        
        var propertyResponse = MapToPropertyResponse(property);

        return Result.Success(propertyResponse);
    }

    public async Task<Result<PropertyResponse>> UpdateProperty(Property oldProperty, Property newProperty)
    {
        var propertyToUpdate = await _propertyRepository.GetProperty(oldProperty.Id);
        if (propertyToUpdate == null)
        {
            return Result.Failure<PropertyResponse>("The property you want to update was not found");
        }
        var oldOwners = propertyToUpdate.Owners;
        propertyToUpdate.ChangeTo(newProperty);
        if (propertyToUpdate.Owners.Count == 0)
        {
            propertyToUpdate.Owners = oldOwners;

        }
        
        var propertyUpdated = await _propertyRepository.UpdateProperty(propertyToUpdate);
        
        if (!propertyUpdated)
        {
            return Result.Failure<PropertyResponse>("Update failed");
        }

        var propertyResponse = MapToPropertyResponse(propertyToUpdate);
        return Result.Success<PropertyResponse>(propertyResponse);
    }

    public async Task<Result> DeleteProperty(Property property)
    {
        
        var propertyToDelete = await _propertyRepository.GetProperty(property.Id);
        if (propertyToDelete == null)
        {
            return Result.Failure("This property does not exist");
        }

        var ownerDeleted = await _propertyRepository.DeleteProperty(propertyToDelete);
        if (ownerDeleted)
        {
            return Result.Success("Property successfully deleted");
        }

        return Result.Failure("Delete failed");
    }

    public async Task<Result> DeactivateProperty(int id)
    {
        var propertyToDeactivate =  await _propertyRepository.GetProperty(id);
        if (propertyToDeactivate == null)
        {
            return Result.Failure("Property Not found");
        }

        propertyToDeactivate.Type = PropertyType.Deactivated;
        var propertyDeactivated = await _propertyRepository.UpdateProperty(propertyToDeactivate);

        return !propertyDeactivated ? Result.Failure("Property Not found") : Result.Success("Property Deactivated");
    }
    
    private PropertyResponse MapToPropertyResponse(Property property)
    {    var ownerDetails = property.Owners.Select(owner => new OwnerDetail
        {
            VatNumber = owner.VatNumber,
            Name = $"{owner.FirstName} {owner.LastName}"
        }).ToList();


        var propertyResponse = new PropertyResponse(property.IdentificationNumber, property.Address,
            property.YearOfConstruction, property.Type, ownerDetails);
            

        return propertyResponse;
    }
}