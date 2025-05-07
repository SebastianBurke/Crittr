using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface ICritterService
{
    Task<List<Critter>> GetAllAsync();
    Task<Critter?> GetByIdAsync(int id);
    Task<CritterDto?> GetDtoByIdAsync(int id);
    Task<List<CritterDto>> GetAllDtosByEnclosureIdAsync(int enclosureId);
    Task<Critter> CreateAsync(Critter critter);
    Task<bool> UpdateAsync(Critter critter);
    Task<bool> DeleteAsync(int id);
    Task<List<CritterDto>> SearchAsync(string searchTerm);
}