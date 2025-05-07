using Microsoft.AspNetCore.Mvc;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<HealthScoreDto>>> GetByCritterId(int critterId)
    {
        return await _healthService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/latest")]
    public async Task<ActionResult<int?>> GetLatestScore(int critterId)
    {
        return await _healthService.GetLatestHealthScoreAsync(critterId);
    }
}