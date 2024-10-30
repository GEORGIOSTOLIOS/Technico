using CSharpFunctionalExtensions;
using Technico.Models;
using Technico.Responses;

namespace Technico.Iservices;

public interface IRepairService
{
     Task<Result<RepairResponse>> CreateRepair (Repair repair, Owner owner);
     Task<Result<RepairResponse>> GetRepair (int id);
     Task<Result<RepairResponse>> UpdateRepair (int oldRepairId, Repair newRepair);
     Task<Result> DeleteRepair (int repairId);
     Task<Result> DeactivateRepair(int id);

}
