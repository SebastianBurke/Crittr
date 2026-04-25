using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EnvironmentController : ControllerBase
{
    private readonly IEnvironmentService _environmentService;
    private readonly ApplicationDbContext _db;

    public EnvironmentController(IEnvironmentService environmentService, ApplicationDbContext db)
    {
        _environmentService = environmentService;
        _db = db;
    }

    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private async Task<bool> OwnsCritterAsync(int critterId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.Critters.AnyAsync(c => c.Id == critterId && c.UserId == uid);
    }

    private async Task<bool> OwnsReadingAsync(int readingId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.EnvironmentalReadings
            .Where(e => e.Id == readingId)
            .Join(_db.Critters, e => e.CritterId, c => c.Id, (_, c) => c)
            .AnyAsync(c => c.UserId == uid);
    }

    [HttpGet]
    public async Task<ActionResult<List<EnvironmentalReading>>> GetAll([FromQuery] int take = 100, [FromQuery] int skip = 0)
    {
        var uid = CurrentUserId;
        if (uid is null) return Unauthorized();

        take = Math.Clamp(take, 1, 200);
        skip = Math.Max(0, skip);

        return await _db.EnvironmentalReadings
            .Include(e => e.Critter)
            .Where(e => e.Critter!.UserId == uid)
            .OrderByDescending(e => e.ReadingDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<EnvironmentalReading>>> GetByCritterId(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _environmentService.GetByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/dto")]
    public async Task<ActionResult<List<EnvironmentalReadingDto>>> GetDtosByCritterId(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _environmentService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnvironmentalReading>> GetById(int id)
    {
        if (!await OwnsReadingAsync(id))
            return Forbid();

        var reading = await _environmentService.GetByIdAsync(id);
        if (reading == null)
            return NotFound();

        return reading;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<EnvironmentalReadingDto>> GetDtoById(int id)
    {
        if (!await OwnsReadingAsync(id))
            return Forbid();

        var readingDto = await _environmentService.GetDtoByIdAsync(id);
        if (readingDto == null)
            return NotFound();

        return readingDto;
    }

    [HttpPost]
    public async Task<ActionResult<EnvironmentalReading>> Create(EnvironmentalReading reading)
    {
        if (!await OwnsCritterAsync(reading.CritterId))
            return Forbid();

        reading.Id = 0;

        var createdReading = await _environmentService.CreateAsync(reading);
        return CreatedAtAction(nameof(GetById), new { id = createdReading.Id }, createdReading);
    }
}
