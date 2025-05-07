using Microsoft.AspNetCore.Mvc;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SheddingController : ControllerBase
{
    private readonly ISheddingService _sheddingService;

    public SheddingController(ISheddingService sheddingService)
    {
        _sheddingService = sheddingService;
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<SheddingRecordDto>>> GetByCritterId(int critterId)
    {
        return await _sheddingService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/last")]
    public async Task<ActionResult<DateTime?>> GetLastSheddingDate(int critterId)
    {
        return await _sheddingService.GetLastSheddingDateAsync(critterId);
    }
}