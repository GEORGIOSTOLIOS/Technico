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

    public async Task<Result<RepairResponse>> UpdateRepair(Repair oldRepair, Repair newRepair)
    {
        var repairToUpdate = await _repairRepository.GetRepair(oldRepair.Id);
        if (repairToUpdate == null)
        {
            return Result.Failure<RepairResponse>("The repair you want to update was not found");
        }

        var oldOwner = oldRepair.Owner;
        repairToUpdate.ChangeTo(newRepair);
        
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

    public async Task<Result> DeleteRepair(Repair repair)
    {
        var repairToDelete = await _repairRepository.GetRepair(repair.Id);
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
    
    private RepairResponse MapToRepairResponse(Repair repair)
    {
       

        var repairResponse = new RepairResponse(repair.DateTime, repair.Type, repair.Description, 
            repair.Address, repair.Status, repair.Cost, repair.Owner);

        return repairResponse;
    }
}