using CSharpFunctionalExtensions;
using LanguageExt;
using Technico.IRepositories;
using Technico.Iservices;
using Technico.Models;
using Technico.Responses;

namespace Technico.ServicesImpl;

public class OwnerServiceImpl: IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;
    
    public OwnerServiceImpl(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }
    public async Task<Result<OwnerResponse>> CreateOwner(Owner owner)
    {
        var ownerCreated = await _ownerRepository.CreateOwner(owner);
        if (!ownerCreated)
        {
            return Result.Failure<OwnerResponse>("Owner already exists");
        }

        var ownerResponse = MapToOwnerResponse(owner);

        return Result.Success(ownerResponse);


    }
    

    public async Task<Result<OwnerResponse>> GetOwner(int id)
    {
        var owner = await _ownerRepository.GetOwner(id);
        if (owner == null)
        {
            return Result.Failure<OwnerResponse>("Owner not found");
        }
        
        var ownerResponse = MapToOwnerResponse(owner);

        return Result.Success(ownerResponse);
    }

    public async Task<Result<OwnerResponse>> UpdateOwner(int oldOwnerId, Owner newOwner)
    {
        var ownerToUpdate = await _ownerRepository.GetOwner(oldOwnerId);
        if (ownerToUpdate == null)
        {
            return Result.Failure<OwnerResponse>("The owner you want to update was not found");
        }

        ownerToUpdate = newOwner;
        
        var ownerUpdated = await _ownerRepository.UpdateOwner(ownerToUpdate);
        
        if (!ownerUpdated)
        {
            return Result.Failure<OwnerResponse>("Update failed");
        }

        var ownerResponse = MapToOwnerResponse(ownerToUpdate);
        return Result.Success<OwnerResponse>(ownerResponse);
    }

    public async Task<Result> DeleteOwner(int ownerId)
    {
        var ownerToDelete = await _ownerRepository.GetOwner(ownerId);
        if (ownerToDelete == null)
        {
            return Result.Failure("This owner does not exist");
        }

        foreach (Property property in ownerToDelete.Properties)
        {
            property.Owners.Remove(ownerToDelete);
        }

        ownerToDelete.Type = OwnerType.None;

        var ownerDeleted = await _ownerRepository.UpdateOwner(ownerToDelete);
        return ownerDeleted ? Result.Success("Owner successfully deleted") : Result.Failure("Delete failed");
    }

    private OwnerResponse MapToOwnerResponse(Owner owner)
    {
        var repairDetails = owner.Repairs.Select(repair => new RepairDetail
        {
            
            Type = repair.Type,
            DateTime = repair.DateTime,
            Description = repair.Description,
            Address = repair.Address,
            Status = repair.Status,
            Cost = repair.Cost
        }).ToList();

        var ownerResponse = new OwnerResponse(owner.VatNumber, owner.FirstName, owner.LastName, 
            owner.Address, owner.PhoneNumber, owner.Email, owner.Properties, repairDetails);

        return ownerResponse;
    }
    
    
    
}