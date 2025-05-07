using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface IEnclosureService
{
    Task<List<EnclosureProfile>> GetAllAsync();
    Task<EnclosureProfile?> GetByIdAsync(int id);
    Task<EnclosureProfileDto?> GetDtoByIdAsync(int id);
    Task<List<EnclosureProfileDto>> GetAllDtosByUserIdAsync(string userId);
    Task<List<EnclosureProfileDto>> GetAllDtosAsync();
    Task<EnclosureProfile> CreateAsync(EnclosureProfile profile);
    Task<bool> UpdateAsync(EnclosureProfile profile);
    Task<bool> DeleteAsync(int id);
    Task<bool> AssignToCritterAsync(int enclosureId, int critterId);
    Task<bool> UnassignFromCritterAsync(int critterId);
}