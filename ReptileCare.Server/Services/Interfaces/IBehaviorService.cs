using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IBehaviorService
{
    Task<List<BehaviorLog>> GetAllAsync();
    Task<List<BehaviorLog>> GetByReptileIdAsync(int reptileId);
    Task<List<BehaviorLogDto>> GetDtosByReptileIdAsync(int reptileId);
    Task<BehaviorLog?> GetByIdAsync(int id);
    Task<BehaviorLogDto?> GetDtoByIdAsync(int id);
    Task<BehaviorLog> CreateAsync(BehaviorLog behaviorLog);
    Task<bool> UpdateAsync(BehaviorLog behaviorLog);
    Task<bool> DeleteAsync(int id);
    Task<List<BehaviorLogDto>> GetRecentAbnormalBehaviorsAsync(int days);
}