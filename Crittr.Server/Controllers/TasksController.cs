using Microsoft.AspNetCore.Mvc;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IScheduledTaskService _taskService;

    public TasksController(IScheduledTaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<ScheduledTaskDto>>> GetByCritterId(int critterId)
    {
        return await _taskService.GetDtosByCritterIdAsync(critterId);
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

    [HttpGet("critter/{critterId}/pending-count")]
    public async Task<ActionResult<int>> GetPendingCount(int critterId)
    {
        return await _taskService.GetPendingTasksCountAsync(critterId);
    }
}