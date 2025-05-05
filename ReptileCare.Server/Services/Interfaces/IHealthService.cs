using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IHealthService
{
    Task<List<HealthScore>> GetAllAsync();
    Task<List<HealthScore>> GetByReptileIdAsync(int reptileId);
    Task<List<HealthScoreDto>> GetDtosByReptileIdAsync(int reptileId);
    Task<HealthScore?> GetByIdAsync(int id);
    Task<HealthScoreDto?> GetDtoByIdAsync(int id);
    Task<HealthScore> CreateAsync(HealthScore healthScore);
    Task<bool> UpdateAsync(HealthScore healthScore);
    Task<bool> DeleteAsync(int id);
    Task<int?> GetLatestHealthScoreAsync(int reptileId);
    Task<List<HealthScoreDto>> GetHealthTrendDataAsync(int reptileId, int months);
}