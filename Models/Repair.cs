using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Technico.Models;

public class Repair
{
    public int Id { get; set; }
    
    public RepairType Type { get; set; }
    
    public DateTime DateTime { get; set; }
    
    [MaxLength(500)] 
    public string? Description { get; set; }

    [MaxLength(200)] 
    public string Address { get; set; } = string.Empty;
    public Status Status { get; set; }
    
    [Precision(10, 2)]
    public decimal Cost { get; set; }
    public Owner? Owner { get; set; } 
    
    
}