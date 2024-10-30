using Technico.Models;
using Technico.Responses;
using LanguageExt;
using CSharpFunctionalExtensions;

namespace Technico.Iservices;

public interface IOwnerService
{
    Task<Result<OwnerResponse>> CreateOwner (Owner owner);
     Task<Result<OwnerResponse>> GetOwner (int id);
     Task<Result<OwnerResponse>> UpdateOwner (int oldOwnerId, Owner newOwner);
     Task<Result> DeleteOwner (int ownerId);
}