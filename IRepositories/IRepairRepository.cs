using Technico.Models;

namespace Technico.IRepositories;

public interface IRepairRepository
{
    public Task<List<Repair>> GetRepairs();

    public Task<Repair?> GetRepair(int id);
    
    public Task<bool> RepairExists(int id);

    Task<bool> CreateRepair(Repair repair, Owner owner);
  
    Task<bool> UpdateRepair( Repair repair);
    
    Task<bool> DeleteRepair(Repair repair);
    
    Task<bool> Save();
}