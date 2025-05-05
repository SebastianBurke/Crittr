using Microsoft.AspNetCore.Mvc;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("reptile/{reptileId}")]
    public async Task<ActionResult<List<HealthScoreDto>>> GetByReptileId(int reptileId)
    {
        return await _healthService.GetDtosByReptileIdAsync(reptileId);
    }

    [HttpGet("reptile/{reptileId}/latest")]
    public async Task<ActionResult<int?>> GetLatestScore(int reptileId)
    {
        return await _healthService.GetLatestHealthScoreAsync(reptileId);
    }
}