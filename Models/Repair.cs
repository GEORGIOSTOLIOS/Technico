namespace Technico.Models;

public class Repair
{
    public int Id { get; set; }
    
    public RepairType Type { get; set; }
    
    public DateTime DateTime { get; set; } 
    
    public string? Description { get; set; }
    
    public string Address { get; set; } = string.Empty;
    
    public Status Status { get; set; }
    
    public decimal Cost { get; set; }

    public Owner Owner { get; set; } = new Owner(){VatNumber = "123"};
    
}