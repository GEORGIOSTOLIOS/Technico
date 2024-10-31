using Microsoft.EntityFrameworkCore;
using Technico.Models;

namespace Technico.Details;

public class RepairDetail
{
    public RepairType Type { get; init; }
    
    public DateTime DateTime { get; init; } 
    
    public string? Description { get; init; }
    
    public string Address { get; init; } = string.Empty;
    
    public Status Status { get; init; }
    
    [Precision(10, 2)]
    public decimal Cost { get; init; }
    
    public override string ToString()
    {
        return $"RepairDetail {{ Type = {Type}, DateTime = {DateTime}, Description = {Description}, " +
               $"Address = {Address}, Status = {Status}, Cost = {Cost:C} }}";
    }
}