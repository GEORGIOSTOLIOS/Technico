using CSharpFunctionalExtensions;
using Technico.Models;
using Technico.Responses;

namespace Technico.Iservices;

public interface IRepairService
{
    public Task<Result<RepairResponse>> CreateRepair (Repair repair, Owner owner);
    
    public Task<Result<RepairResponse>> GetRepair (int id);
    
    public Task<Result<RepairResponse>> UpdateRepair (Repair oldRepair, Repair newRepair);
    
    public Task<Result> DeleteRepair (Repair repair);
}
