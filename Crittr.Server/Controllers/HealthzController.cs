using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;

namespace Crittr.Server.Controllers;

/// <summary>
/// Liveness/readiness endpoint for nginx upstream checks and external monitoring.
/// Anonymous on purpose — health probes must not require auth.
/// </summary>
[ApiController]
[Route("api/healthz")]
[AllowAnonymous]
public class HealthzController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public HealthzController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [HttpHead]
    public async Task<IActionResult> Get()
    {
        try
        {
            await _db.Database.ExecuteSqlRawAsync("SELECT 1");
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
        catch
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = "unhealthy" });
        }
    }
}
