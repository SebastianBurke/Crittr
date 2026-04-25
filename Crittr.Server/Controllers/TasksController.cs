using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TasksController : ControllerBase
{
    private readonly IScheduledTaskService _taskService;
    private readonly ApplicationDbContext _db;

    public TasksController(IScheduledTaskService taskService, ApplicationDbContext db)
    {
        _taskService = taskService;
        _db = db;
    }

    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private async Task<bool> OwnsCritterAsync(int critterId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.Critters.AnyAsync(c => c.Id == critterId && c.UserId == uid);
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<ScheduledTaskDto>>> GetByCritterId(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _taskService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("upcoming/{days}")]
    public async Task<ActionResult<List<ScheduledTaskDto>>> GetUpcoming(int days)
    {
        var uid = CurrentUserId;
        if (uid is null) return Unauthorized();

        return await _taskService.GetUpcomingTasksByUserAsync(uid, days);
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<List<ScheduledTaskDto>>> GetOverdue()
    {
        var uid = CurrentUserId;
        if (uid is null) return Unauthorized();

        return await _taskService.GetOverdueTasksByUserAsync(uid);
    }

    [HttpGet("critter/{critterId}/pending-count")]
    public async Task<ActionResult<int>> GetPendingCount(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _taskService.GetPendingTasksCountAsync(critterId);
    }
}
