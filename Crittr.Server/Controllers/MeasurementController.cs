using Microsoft.AspNetCore.Mvc;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementController : ControllerBase
{
    private readonly IMeasurementService _measurementService;

    public MeasurementController(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<MeasurementRecordDto>>> GetByCritterId(int critterId)
    {
        return await _measurementService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/latest")]
    public async Task<ActionResult<MeasurementRecordDto?>> GetLatest(int critterId)
    {
        return await _measurementService.GetLatestForCritterAsync(critterId);
    }
}