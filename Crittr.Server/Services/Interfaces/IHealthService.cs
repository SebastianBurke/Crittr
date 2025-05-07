using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface IHealthService
{
    Task<List<HealthScore>> GetAllAsync();
    Task<List<HealthScore>> GetByCritterIdAsync(int critterId);
    Task<List<HealthScoreDto>> GetDtosByCritterIdAsync(int critterId);
    Task<HealthScore?> GetByIdAsync(int id);
    Task<HealthScoreDto?> GetDtoByIdAsync(int id);
    Task<HealthScore> CreateAsync(HealthScore healthScore);
    Task<bool> UpdateAsync(HealthScore healthScore);
    Task<bool> DeleteAsync(int id);
    Task<int?> GetLatestHealthScoreAsync(int critterId);
    Task<List<HealthScoreDto>> GetHealthTrendDataAsync(int critterId, int months);
}