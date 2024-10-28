using Microsoft.EntityFrameworkCore;
using Technico.Data;
using Technico.IRepositories;
using Technico.Models;

namespace Technico.Repositories;

public class OwnerRepository: IOwnerRepository
{
    private readonly TechnicoDbContext _context;
    
    public OwnerRepository(TechnicoDbContext context): base()
    {
        _context = context;
    }
    public async Task<List<Owner>> GetOwners()
    {
       return await _context.Owners.OrderBy(o => o.LastName).ToListAsync();
    }

    public async Task<Owner?> GetOwner(int id)
    {
        return await _context.Owners.Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> OwnerExists( string vatNumber)
    {
        return await _context.Owners.AnyAsync(o =>  o.VatNumber.Equals(vatNumber.Trim()));
    }

    public async Task<bool> CreateOwner(Owner owner)
    {
        if (!OwnerExists( owner.VatNumber).Result)
        {
            _context.Add(owner);
        }

        return await Save();
    }

    public async Task<bool> UpdateOwner( Owner owner)
    {
        _context.Update(owner);
        return await Save();
    }

    public async Task<bool> DeleteOwner(Owner owner)
    {
        if (OwnerExists( owner.VatNumber).Result)
        {
            _context.Remove(owner);
        }

        return await Save();
    }

    public async Task<bool> Save()
    {
        var saved = _context.SaveChangesAsync();
        return await saved > 0;
    }
}