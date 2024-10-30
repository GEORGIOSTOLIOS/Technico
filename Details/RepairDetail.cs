using Microsoft.EntityFrameworkCore;

namespace Technico.Models;

public class RepairDetail
{
    public RepairType Type { get; set; }
    
    public DateTime DateTime { get; set; } 
    
    public string? Description { get; set; }
    
    public string Address { get; set; } = string.Empty;
    
    public Status Status { get; set; }
    
    [Precision(10, 2)]
    public decimal Cost { get; set; }
    
    public override string ToString()
    {
        return $"RepairDetail {{ Type = {Type}, DateTime = {DateTime}, Description = {Description}, " +
               $"Address = {Address}, Status = {Status}, Cost = {Cost:C} }}";
    }
}