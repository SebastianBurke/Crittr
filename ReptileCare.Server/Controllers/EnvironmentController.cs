using Microsoft.AspNetCore.Mvc;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnvironmentController : ControllerBase
{
    private readonly IEnvironmentService _environmentService;

    public EnvironmentController(IEnvironmentService environmentService)
    {
        _environmentService = environmentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EnvironmentalReading>>> GetAll()
    {
        return await _environmentService.GetAllAsync();
    }

    [HttpGet("reptile/{reptileId}")]
    public async Task<ActionResult<List<EnvironmentalReading>>> GetByReptileId(int reptileId)
    {
        return await _environmentService.GetByReptileIdAsync(reptileId);
    }

    [HttpGet("reptile/{reptileId}/dto")]
    public async Task<ActionResult<List<EnvironmentalReadingDto>>> GetDtosByReptileId(int reptileId)
    {
        return await _environmentService.GetDtosByReptileIdAsync(reptileId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnvironmentalReading>> GetById(int id)
    {
        var reading = await _environmentService.GetByIdAsync(id);
        if (reading == null)
            return NotFound();

        return reading;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<EnvironmentalReadingDto>> GetDtoById(int id)
    {
        var readingDto = await _environmentService.GetDtoByIdAsync(id);
        if (readingDto == null)
            return NotFound();

        return readingDto;
    }

    [HttpPost]
    public async Task<ActionResult<EnvironmentalReading>> Create(EnvironmentalReading reading)
    {
        var createdReading = await _environmentService.CreateAsync(reading);
        return CreatedAtAction(nameof(GetById), new { id = createdReading.Id }, createdReading);
    }
}
