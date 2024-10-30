﻿namespace Technico.Models;

public class OwnerDetail
{
    public required string VatNumber { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    
    public override string ToString()
    {
        return $"OwnerDetail: VAT = {VatNumber}, Name = {Name}";
    }
}