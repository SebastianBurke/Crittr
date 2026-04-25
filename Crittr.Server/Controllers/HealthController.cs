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
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;
    private readonly ApplicationDbContext _db;

    public HealthController(IHealthService healthService, ApplicationDbContext db)
    {
        _healthService = healthService;
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
    public async Task<ActionResult<List<HealthScoreDto>>> GetByCritterId(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _healthService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/latest")]
    public async Task<ActionResult<int?>> GetLatestScore(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _healthService.GetLatestHealthScoreAsync(critterId);
    }
}
