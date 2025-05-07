using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface IEnvironmentService
{
    Task<List<EnvironmentalReading>> GetAllAsync();
    Task<List<EnvironmentalReading>> GetByCritterIdAsync(int critterId);
    Task<List<EnvironmentalReadingDto>> GetDtosByCritterIdAsync(int critterId);
    Task<EnvironmentalReading?> GetByIdAsync(int id);
    Task<EnvironmentalReadingDto?> GetDtoByIdAsync(int id);
    Task<EnvironmentalReading> CreateAsync(EnvironmentalReading reading);
    Task<bool> UpdateAsync(EnvironmentalReading reading);
    Task<bool> DeleteAsync(int id);
    Task<List<EnvironmentalReadingDto>> GetLatestReadingsAsync(int count);
    Task<EnvironmentalReadingDto?> GetLatestForCritterAsync(int critterId);
}