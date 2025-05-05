using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IReptileService
{
    Task<List<Reptile>> GetAllAsync();
    Task<Reptile?> GetByIdAsync(int id);
    Task<ReptileDto?> GetDtoByIdAsync(int id);
    Task<List<ReptileDto>> GetAllDtosAsync();
    Task<Reptile> CreateAsync(Reptile reptile);
    Task<bool> UpdateAsync(Reptile reptile);
    Task<bool> DeleteAsync(int id);
    Task<List<ReptileDto>> SearchAsync(string searchTerm);
}