using Technico.Models;

namespace Technico.IRepositories;

public interface IRepairRepository
{
    Task<List<Repair>> GetRepairs();
    Task<Repair?> GetRepair(int id);
    Task<bool> RepairExists(int id);
    Task<bool> CreateRepair(Repair repair, Owner owner);
    Task<bool> UpdateRepair( Repair repair);
    Task<bool> DeleteRepair(Repair repair);
    Task<bool> Save();
}