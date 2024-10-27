using Technico.Models;
using Technico.Responses;
using LanguageExt;
using CSharpFunctionalExtensions;

namespace Technico.Iservices;

public interface IOwnerService
{
    public Task<Result<OwnerResponse>> CreateOwner (Owner owner);
    public Task<Result<OwnerResponse>> GetOwner (int id);
    public Task<Result<OwnerResponse>> UpdateOwner (Owner oldOwner, Owner newOwner);
    public Task<Result> DeleteOwner (Owner owner);
}