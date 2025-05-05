using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.Data;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services;

public class ScheduledTaskService : IScheduledTaskService
{
    private readonly ApplicationDbContext _db;

    public ScheduledTaskService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<ScheduledTask>> GetAllAsync()
    {
        return await _db.ScheduledTasks
            .Include(t => t.Reptile)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<List<ScheduledTask>> GetByReptileIdAsync(int reptileId)
    {
        return await _db.ScheduledTasks
            .Include(t => t.Reptile)
            .Where(t => t.ReptileId == reptileId)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<List<ScheduledTaskDto>> GetDtosByReptileIdAsync(int reptileId)
    {
        var tasks = await GetByReptileIdAsync(reptileId);
        return tasks.Select(CreateDtoFromTask).ToList();
    }

    public async Task<ScheduledTask?> GetByIdAsync(int id)
    {
        return await _db.ScheduledTasks
            .Include(t => t.Reptile)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<ScheduledTaskDto?> GetDtoByIdAsync(int id)
    {
        var task = await GetByIdAsync(id);
        return task != null ? CreateDtoFromTask(task) : null;
    }

    public async Task<ScheduledTask> CreateAsync(ScheduledTask scheduledTask)
    {
        await _db.ScheduledTasks.AddAsync(scheduledTask);
        await _db.SaveChangesAsync();
        return scheduledTask;
    }

    public async Task<bool> UpdateAsync(ScheduledTask scheduledTask)
    {
        var existing = await _db.ScheduledTasks.FindAsync(scheduledTask.Id);
        if (existing == null) return false;

        _db.Entry(existing).CurrentValues.SetValues(scheduledTask);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _db.ScheduledTasks.FindAsync(id);
        if (task == null) return false;

        _db.ScheduledTasks.Remove(task);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteTaskAsync(int id)
    {
        var task = await _db.ScheduledTasks.FindAsync(id);
        if (task == null || task.IsCompleted) return false;

        task.IsCompleted = true;
        task.CompletedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ScheduledTaskDto>> GetUpcomingTasksAsync(int days)
    {
        var threshold = DateTime.UtcNow.AddDays(days);
        var tasks = await _db.ScheduledTasks
            .Include(t => t.Reptile)
            .Where(t => !t.IsCompleted && t.DueDate <= threshold)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return tasks.Select(CreateDtoFromTask).ToList();
    }

    public async Task<List<ScheduledTaskDto>> GetOverdueTasksAsync()
    {
        var now = DateTime.UtcNow;
        var tasks = await _db.ScheduledTasks
            .Include(t => t.Reptile)
            .Where(t => !t.IsCompleted && t.DueDate < now)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return tasks.Select(CreateDtoFromTask).ToList();
    }

    public async Task<int> GetPendingTasksCountAsync(int reptileId)
    {
        return await _db.ScheduledTasks
            .Where(t => t.ReptileId == reptileId && !t.IsCompleted)
            .CountAsync();
    }

    private ScheduledTaskDto CreateDtoFromTask(ScheduledTask task)
    {
        return new ScheduledTaskDto
        {
            Id = task.Id,
            ReptileId = task.ReptileId,
            ReptileName = task.Reptile?.Name ?? string.Empty,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            Priority = task.Priority,
            CompletedDate = task.CompletedDate
        };
    }
}
