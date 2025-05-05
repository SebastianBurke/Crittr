using Microsoft.AspNetCore.Mvc;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementController : ControllerBase
{
    private readonly IMeasurementService _measurementService;

    public MeasurementController(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    [HttpGet("reptile/{reptileId}")]
    public async Task<ActionResult<List<MeasurementRecordDto>>> GetByReptileId(int reptileId)
    {
        return await _measurementService.GetDtosByReptileIdAsync(reptileId);
    }

    [HttpGet("reptile/{reptileId}/latest")]
    public async Task<ActionResult<MeasurementRecordDto?>> GetLatest(int reptileId)
    {
        return await _measurementService.GetLatestForReptileAsync(reptileId);
    }
}