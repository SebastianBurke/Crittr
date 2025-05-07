using Microsoft.AspNetCore.Mvc;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedingController : ControllerBase
{
    private readonly IFeedingService _feedingService;

    public FeedingController(IFeedingService feedingService)
    {
        _feedingService = feedingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FeedingRecord>>> GetAll()
    {
        return await _feedingService.GetAllAsync();
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<FeedingRecord>>> GetByCritterId(int critterId)
    {
        return await _feedingService.GetByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/dto")]
    public async Task<ActionResult<List<FeedingRecordDto>>> GetDtosByCritterId(int critterId)
    {
        return await _feedingService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FeedingRecord>> GetById(int id)
    {
        var record = await _feedingService.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        return record;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<FeedingRecordDto>> GetDtoById(int id)
    {
        var recordDto = await _feedingService.GetDtoByIdAsync(id);
        if (recordDto == null)
            return NotFound();

        return recordDto;
    }

    [HttpPost]
    public async Task<ActionResult<FeedingRecord>> Create(FeedingRecord feedingRecord)
    {
        var createdRecord = await _feedingService.CreateAsync(feedingRecord);
        return CreatedAtAction(nameof(GetById), new { id = createdRecord.Id }, createdRecord);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FeedingRecord feedingRecord)
    {
        if (id != feedingRecord.Id)
            return BadRequest();

        var success = await _feedingService.UpdateAsync(feedingRecord);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _feedingService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("critter/{critterId}/last")]
    public async Task<ActionResult<DateTime?>> GetLastFeedingDate(int critterId)
    {
        return await _feedingService.GetLastFeedingDateAsync(critterId);
    }
}