﻿using Technico.Models;

namespace Technico.IRepositories;

public interface IPropertyRepository
{
    Task<List<Property>> GetProperties(); 
    Task<Property?> GetProperty(int id);
    Task<bool> PropertyExists(int id);
    Task<bool> CreateProperty(Property property, List<string> propertyOwnersVatNumbers);
    Task<bool> UpdateProperty( Property property);
    Task<bool> DeleteProperty(Property property);
    Task<bool> Save();
    
}