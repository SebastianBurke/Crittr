using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface IBehaviorService
{
    Task<List<BehaviorLog>> GetAllAsync();
    Task<List<BehaviorLog>> GetByCritterIdAsync(int critterId);
    Task<List<BehaviorLogDto>> GetDtosByCritterIdAsync(int critterId);
    Task<BehaviorLog?> GetByIdAsync(int id);
    Task<BehaviorLogDto?> GetDtoByIdAsync(int id);
    Task<BehaviorLog> CreateAsync(BehaviorLog behaviorLog);
    Task<bool> UpdateAsync(BehaviorLog behaviorLog);
    Task<bool> DeleteAsync(int id);
    Task<List<BehaviorLogDto>> GetRecentAbnormalBehaviorsAsync(int days);
}