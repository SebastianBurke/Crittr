using Crittr.Shared.DTOs;
using Crittr.Shared.Models;
using Crittr.Shared.Models.Enums;

namespace Crittr.Server.Services.Interfaces;

public interface IEnclosureService
{
    Task<EnclosureProfile?> GetByIdAsync(int id);
    Task<EnclosureProfile?> GetByIdAsync(int id, string ownerId);
    Task<EnclosureProfileDto?> GetDtoByIdAsync(int id);
    Task<EnclosureProfileDto?> GetDtoByIdAsync(int id, string ownerId);
    Task<List<EnclosureProfileDto>> GetAllDtosByUserIdAsync(string userId);
    Task<List<EnclosureProfileDto>> GetAllDtosByUserIdAsync(string userId, int take, int skip);
    Task<List<EnclosureProfileDto>> GetCompatibleByUserIdAsync(string userId, SpeciesType speciesType);
    Task<EnclosureProfile> CreateAsync(EnclosureProfile profile);
    Task<bool> UpdateFromDtoAsync(EnclosureProfileDto dto, string ownerId);
    Task<bool> DeleteAsync(int id, string ownerId);
    Task<bool> AssignToCritterAsync(int enclosureId, int critterId);
    Task<bool> UnassignFromCritterAsync(int critterId);
}
