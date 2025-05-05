using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IScheduledTaskService
{
    Task<List<ScheduledTask>> GetAllAsync();
    Task<List<ScheduledTask>> GetByReptileIdAsync(int reptileId);
    Task<List<ScheduledTaskDto>> GetDtosByReptileIdAsync(int reptileId);
    Task<ScheduledTask?> GetByIdAsync(int id);
    Task<ScheduledTaskDto?> GetDtoByIdAsync(int id);
    Task<ScheduledTask> CreateAsync(ScheduledTask scheduledTask);
    Task<bool> UpdateAsync(ScheduledTask scheduledTask);
    Task<bool> DeleteAsync(int id);
    Task<bool> CompleteTaskAsync(int id);
    Task<List<ScheduledTaskDto>> GetUpcomingTasksAsync(int days);
    Task<List<ScheduledTaskDto>> GetOverdueTasksAsync();
    Task<int> GetPendingTasksCountAsync(int reptileId);
}