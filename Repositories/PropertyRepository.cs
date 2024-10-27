﻿using Microsoft.EntityFrameworkCore;
using Technico.Data;
using Technico.IRepositories;
using Technico.Models;

namespace Technico.Repositories;

public class PropertyRepository:IPropertyRepository
{    private readonly TechnicoDbContext _context;

    public PropertyRepository(TechnicoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Property>> GetProperties()
    {
        return await _context.Properties.OrderBy(p => p.YearOfConstruction).ToListAsync();
    }

    public async Task<Property> GetProperty(int id)
    {
        return await _context.Properties.Where(p => p.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<bool> PropertyExists(int id)
    {
        return await _context.Properties.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> CreateProperty(Property property, PropertyType propertyType, List<string> propertyOwnersVatNumbers)
    {
        var owners = _context.Owners
            .Where(owner => propertyOwnersVatNumbers.Contains(owner.VatNumber))
            .ToList();

        property.Owners = owners;
        _context.Add(property);

        return await Save();

    }

    public async Task<bool> UpdateProperty(Property property)
    {
        _context.Update(property);
        return await Save();
    }

    public async Task<bool> DeleteProperty(Property property)
    {
        if (PropertyExists(property.Id).Result)
        {
            _context.Remove(property);
        }

        return await Save();
    }

    public async Task<bool> Save()
    {
        var saved = _context.SaveChangesAsync();
        return await saved > 0;
    }
}