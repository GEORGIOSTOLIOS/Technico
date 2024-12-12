using Microsoft.EntityFrameworkCore;
using Technico.Data;
using Technico.IRepositories;
using Technico.Models;

namespace Technico.Repositories;

public class RepairRepository: IRepairRepository
{ private readonly TechnicoDbContext _context;
    private readonly IOwnerRepository _ownerRepository;

    public RepairRepository(TechnicoDbContext context, IOwnerRepository ownerRepository)
    {
        _context = context;
        _ownerRepository = ownerRepository;
    }
    
    public async Task<List<Repair>> GetRepairs()
    {
        return await _context.Repairs.OrderBy(r => r.DateTime).Include(r => r.Owner).ToListAsync();
    }

    public async Task<Repair?> GetRepair(int id)
    {
        return await _context.Repairs.Where(r => r.Id == id).
            Include(r => r.Owner).FirstOrDefaultAsync();
    }

    public async Task<bool> RepairExists(int id)
    {
        return await _context.Repairs.AnyAsync(r => r.Id == id);
    }

    public async Task<bool> CreateRepair(Repair repair, Owner owner)
    {
        var ownerExists = await _ownerRepository.OwnerExists(owner.VatNumber);
        if (!ownerExists)
        {
            return false;
        }

        repair.Owner = owner;
        _context.Add(repair);

        return await Save();
    }

    public async Task<bool> UpdateRepair(Repair repair)
    { 
        _context.ChangeTracker.Clear();
        _context.Update(repair);
        return await Save();
    }

    public Task<bool> DeleteRepair(Repair repair)
    { 
        _context.Remove(repair);
        return Save();
    }

    public async Task<bool> Save()
    {
        var saved = _context.SaveChangesAsync();
        return await saved > 0;
    }
}