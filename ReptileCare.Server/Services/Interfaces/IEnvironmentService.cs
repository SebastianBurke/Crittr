using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IEnvironmentService
{
    Task<List<EnvironmentalReading>> GetAllAsync();
    Task<List<EnvironmentalReading>> GetByReptileIdAsync(int reptileId);
    Task<List<EnvironmentalReadingDto>> GetDtosByReptileIdAsync(int reptileId);
    Task<EnvironmentalReading?> GetByIdAsync(int id);
    Task<EnvironmentalReadingDto?> GetDtoByIdAsync(int id);
    Task<EnvironmentalReading> CreateAsync(EnvironmentalReading reading);
    Task<bool> UpdateAsync(EnvironmentalReading reading);
    Task<bool> DeleteAsync(int id);
    Task<List<EnvironmentalReadingDto>> GetLatestReadingsAsync(int count);
    Task<EnvironmentalReadingDto?> GetLatestForReptileAsync(int reptileId);
}