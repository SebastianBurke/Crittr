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
public class FeedingController : ControllerBase
{
    private readonly IFeedingService _feedingService;
    private readonly ApplicationDbContext _db;

    public FeedingController(IFeedingService feedingService, ApplicationDbContext db)
    {
        _feedingService = feedingService;
        _db = db;
    }

    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private async Task<bool> OwnsCritterAsync(int critterId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.Critters.AnyAsync(c => c.Id == critterId && c.UserId == uid);
    }

    private async Task<bool> OwnsFeedingRecordAsync(int feedingId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.FeedingRecords
            .Where(f => f.Id == feedingId)
            .Join(_db.Critters, f => f.CritterId, c => c.Id, (_, c) => c)
            .AnyAsync(c => c.UserId == uid);
    }

    [HttpGet]
    public async Task<ActionResult<List<FeedingRecordDto>>> GetAll()
    {
        var uid = CurrentUserId;
        if (uid is null) return Unauthorized();

        var myCritterIds = await _db.Critters.Where(c => c.UserId == uid).Select(c => c.Id).ToListAsync();
        var records = await _feedingService.GetAllAsync();
        var filtered = records.Where(f => myCritterIds.Contains(f.CritterId)).ToList();
        return filtered.Select(MapToDto).ToList();
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<FeedingRecord>>> GetByCritterId(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _feedingService.GetByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/dto")]
    public async Task<ActionResult<List<FeedingRecordDto>>> GetDtosByCritterId(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _feedingService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FeedingRecord>> GetById(int id)
    {
        if (!await OwnsFeedingRecordAsync(id))
            return Forbid();

        var record = await _feedingService.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        return record;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<FeedingRecordDto>> GetDtoById(int id)
    {
        if (!await OwnsFeedingRecordAsync(id))
            return Forbid();

        var recordDto = await _feedingService.GetDtoByIdAsync(id);
        if (recordDto == null)
            return NotFound();

        return recordDto;
    }

    [HttpPost]
    public async Task<ActionResult<FeedingRecord>> Create([FromBody] FeedingRecord feedingRecord)
    {
        if (!await OwnsCritterAsync(feedingRecord.CritterId))
            return Forbid();

        var critter = await _db.Critters.FindAsync(feedingRecord.CritterId);
        if (critter?.EnclosureProfileId == null)
            return BadRequest("Critter must be assigned to an enclosure before logging a feeding.");

        feedingRecord.Id = 0;
        feedingRecord.Critter = null;

        var createdRecord = await _feedingService.CreateAsync(feedingRecord);
        return CreatedAtAction(nameof(GetById), new { id = createdRecord.Id }, createdRecord);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FeedingRecord feedingRecord)
    {
        if (id != feedingRecord.Id)
            return BadRequest();

        if (!await OwnsFeedingRecordAsync(id) || !await OwnsCritterAsync(feedingRecord.CritterId))
            return Forbid();

        feedingRecord.Critter = null;

        var success = await _feedingService.UpdateAsync(feedingRecord);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await OwnsFeedingRecordAsync(id))
            return Forbid();

        var success = await _feedingService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("critter/{critterId}/last")]
    public async Task<ActionResult<DateTime?>> GetLastFeedingDate(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _feedingService.GetLastFeedingDateAsync(critterId);
    }

    private static FeedingRecordDto MapToDto(FeedingRecord record) =>
        new()
        {
            Id = record.Id,
            CritterId = record.CritterId,
            CritterName = record.Critter?.Name ?? string.Empty,
            FeedingDate = record.FeedingDate,
            FoodItem = record.FoodItem,
            Quantity = record.Quantity,
            ItemType = record.ItemType,
            WasEaten = record.WasEaten,
            Notes = record.Notes
        };
}
