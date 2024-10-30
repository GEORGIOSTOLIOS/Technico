using Technico.Models;

namespace Technico.IRepositories;

public interface IOwnerRepository
{  Task<List<Owner>> GetOwners();
   Task<Owner?>  GetOwner(int id);
   Task<bool> OwnerExists( string vatNumber);
   Task<bool> CreateOwner(Owner owner);
   Task<bool> UpdateOwner( Owner owner);
   Task<bool> DeleteOwner(Owner owner);
   Task<bool> Save();
}