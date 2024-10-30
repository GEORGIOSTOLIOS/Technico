using CSharpFunctionalExtensions;
using Technico.IRepositories;
using Technico.Iservices;
using Technico.Models;
using Technico.Responses;

namespace Technico.ServicesImpl;

public class RepairServiceImpl: IRepairService
{
    private readonly IRepairRepository _repairRepository;

    public RepairServiceImpl(IRepairRepository repairRepository)
    {
        _repairRepository = repairRepository;
    }

    public async Task<Result<RepairResponse>> CreateRepair(Repair repair, Owner owner)
    {
        var repairCreated = await _repairRepository.CreateRepair(repair, owner);
        if (!repairCreated)
        {
            return Result.Failure<RepairResponse>("Owner does not exist");
        }

        var repairResponse = MapToRepairResponse(repair);

        return Result.Success(repairResponse);
    }

    public async Task<Result<RepairResponse>> GetRepair(int id)
    {
        var repair = await _repairRepository.GetRepair(id);
        if (repair == null)
        {
            return Result.Failure<RepairResponse>("Repair not found");
        }
        
        var repairResponse = MapToRepairResponse(repair);

        return Result.Success(repairResponse);
    }

    public async Task<Result<RepairResponse>> UpdateRepair(int oldRepairId, Repair newRepair)
    {
        var repairToUpdate = await _repairRepository.GetRepair(oldRepairId);
        if (repairToUpdate == null)
        {
            return Result.Failure<RepairResponse>("The repair you want to update was not found");
        }

        var oldOwner = repairToUpdate.Owner;
        repairToUpdate = newRepair;
        
        if (newRepair.Owner == null)
        {
            repairToUpdate.Owner = oldOwner;
        }
        
        var repairUpdated = await _repairRepository.UpdateRepair(repairToUpdate);
        
        if (!repairUpdated)
        {
            return Result.Failure<RepairResponse>("Update failed");
        }

        var repairResponse = MapToRepairResponse(repairToUpdate);
        return Result.Success<RepairResponse>(repairResponse);
    }

    public async Task<Result> DeleteRepair(int repairId)
    {
        var repairToDelete = await _repairRepository.GetRepair(repairId);
        if (repairToDelete == null)
        {
            return Result.Failure("This repair does not exist");
        }

        var repairDeleted = await _repairRepository.DeleteRepair(repairToDelete);
        if (repairDeleted)
        {
            return Result.Success("Repair successfully deleted");
        }

        return Result.Failure("Delete failed");
    }
    
    public async Task<Result> DeactivateRepair(int id)
    {
        var repairToDeactivate =  await _repairRepository.GetRepair(id);
        if (repairToDeactivate == null)
        {
            return Result.Failure("Repair Not found");
        }

        repairToDeactivate.Status = Status.Deactivated;
        var propertyDeactivated = await _repairRepository.UpdateRepair(repairToDeactivate);

        return !propertyDeactivated ? Result.Failure("Repair Not found") : Result.Success("Repair Deactivated");
    }
    
    private static RepairResponse MapToRepairResponse(Repair repair)
    {
        var ownerDetails = new OwnerDetail()
        {
            VatNumber = repair.Owner.VatNumber,
            Name = $"{repair.Owner.FirstName} {repair.Owner.LastName}"
        };
       

        var repairResponse = new RepairResponse(repair.DateTime, repair.Type, repair.Description, 
            repair.Address, repair.Status, repair.Cost, ownerDetails);

        return repairResponse;
    }
}