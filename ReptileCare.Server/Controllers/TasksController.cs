using Microsoft.AspNetCore.Mvc;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IScheduledTaskService _taskService;

    public TasksController(IScheduledTaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("reptile/{reptileId}")]
    public async Task<ActionResult<List<ScheduledTaskDto>>> GetByReptileId(int reptileId)
    {
        return await _taskService.GetDtosByReptileIdAsync(reptileId);
    }

    [HttpGet("upcoming/{days}")]
    public async Task<ActionResult<List<ScheduledTaskDto>>> GetUpcoming(int days)
    {
        return await _taskService.GetUpcomingTasksAsync(days);
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<List<ScheduledTaskDto>>> GetOverdue()
    {
        return await _taskService.GetOverdueTasksAsync();
    }

    [HttpGet("reptile/{reptileId}/pending-count")]
    public async Task<ActionResult<int>> GetPendingCount(int reptileId)
    {
        return await _taskService.GetPendingTasksCountAsync(reptileId);
    }
}