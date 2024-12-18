﻿using CSharpFunctionalExtensions;
using Technico.Details;
using Technico.IRepositories;
using Technico.Iservices;
using Technico.Models;
using Technico.Responses;
using Technico.Validators;

namespace Technico.ServicesImpl;

public class OwnerServiceImpl: IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly OwnerValidator _ownerValidator;
    
    public OwnerServiceImpl(IOwnerRepository ownerRepository, OwnerValidator ownerValidator)
    {
        _ownerRepository = ownerRepository;
        _ownerValidator = ownerValidator;
    }
    public async Task<Result<OwnerResponse>> CreateOwner(Owner owner)
    {   
        if (!(await _ownerValidator.ValidateAsync(owner)).IsValid)
        {
            return Result.Failure<OwnerResponse>("Invalid input");
        }
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
        if (!(await _ownerValidator.ValidateAsync(newOwner)).IsValid)
        {
            return Result.Failure<OwnerResponse>("Invalid input");
        }
        
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
        return Result.Success(ownerResponse);
    }

    public async Task<Result> DeleteOwner(int ownerId)
    {
        var ownerToDelete = await _ownerRepository.GetOwner(ownerId);
        if (ownerToDelete == null)
        {
            return Result.Failure("This owner does not exist");
        }
        
        var ownerDeleted = await _ownerRepository.DeleteOwner(ownerToDelete);
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