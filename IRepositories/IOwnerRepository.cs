using Technico.Models;

namespace Technico.IRepositories;

public interface IOwnerRepository
{
  public  Task<List<Owner>> GetOwners();
  public Task<Owner?>  GetOwner(int id);
  public Task<bool> OwnerExists( string vatNumber);
  public Task<bool> CreateOwner(Owner owner);
  public Task<bool> UpdateOwner( Owner owner);
  public Task<bool> DeleteOwner(Owner owner);
  public Task<bool> Save();
}