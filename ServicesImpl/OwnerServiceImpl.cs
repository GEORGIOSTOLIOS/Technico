﻿using CSharpFunctionalExtensions;
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

    public  async Task<Result<OwnerResponse>> UpdateOwner(Owner oldOwner, Owner newOwner)
    {
        

        var ownerToUpdate = await _ownerRepository.GetOwner(oldOwner.Id);
        if (ownerToUpdate == null)
        {
            return Result.Failure<OwnerResponse>("The owner you want to update was not found");
        }
        ownerToUpdate.FirstName = newOwner.FirstName;
        ownerToUpdate.LastName = newOwner.LastName;
        ownerToUpdate.VatNumber = newOwner.VatNumber;
        ownerToUpdate.Address = newOwner.Address;
        ownerToUpdate.Properties = newOwner.Properties;
        ownerToUpdate.Email = newOwner.Email;
        ownerToUpdate.Password = newOwner.Password;
        ownerToUpdate.Repairs = newOwner.Repairs;
        ownerToUpdate.Type = newOwner.Type;
        ownerToUpdate.PhoneNumber = newOwner.PhoneNumber;

        var ownerUpdated = await _ownerRepository.UpdateOwner(ownerToUpdate);
        
        if (!ownerUpdated)
        {
            return Result.Failure<OwnerResponse>("Update failed");
        }

        var ownerResponse = MapToOwnerResponse(ownerToUpdate);
        return Result.Success<OwnerResponse>(ownerResponse);
    }

    public async Task<Result> DeleteOwner(Owner owner)
    {
        var ownerToDelete = await _ownerRepository.GetOwner(owner.Id);
        if (ownerToDelete == null)
        {
            return Result.Failure("This owner does not exist");
        }

        var ownerDeleted = await _ownerRepository.DeleteOwner(ownerToDelete);
        if (ownerDeleted)
        {
            return Result.Success("Owner succesufully deleted");
        }

        return Result.Failure("Delete failed");
    }

    private OwnerResponse MapToOwnerResponse(Owner owner)
    {
       

        var ownerResponse = new OwnerResponse(owner.VatNumber, owner.FirstName, owner.LastName, 
            owner.Address, owner.PhoneNumber, owner.Email, owner.Properties, owner.Repairs);

        return ownerResponse;
    }
}