using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface IScheduledTaskService
{
    Task<List<ScheduledTask>> GetAllAsync();
    Task<List<ScheduledTask>> GetByCritterIdAsync(int critterId);
    Task<List<ScheduledTaskDto>> GetDtosByCritterIdAsync(int critterId);
    Task<ScheduledTask?> GetByIdAsync(int id);
    Task<ScheduledTaskDto?> GetDtoByIdAsync(int id);
    Task<ScheduledTask> CreateAsync(ScheduledTask scheduledTask);
    Task<bool> UpdateAsync(ScheduledTask scheduledTask);
    Task<bool> DeleteAsync(int id);
    Task<bool> CompleteTaskAsync(int id);
    Task<List<ScheduledTaskDto>> GetUpcomingTasksAsync(int days);
    Task<List<ScheduledTaskDto>> GetOverdueTasksAsync();
    Task<int> GetPendingTasksCountAsync(int critterId);
}