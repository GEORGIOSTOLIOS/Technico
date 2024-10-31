using Technico.Details;
using Technico.Models;

namespace Technico.Responses;

public record RepairResponse(
    DateTime DateTime,
    RepairType RepairType,
    string? Description,
    String? Address,
    Status Status,
    decimal Cost,
    OwnerDetail? Owner)
{
    
}