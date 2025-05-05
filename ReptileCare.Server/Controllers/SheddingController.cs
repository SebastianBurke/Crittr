using Microsoft.AspNetCore.Mvc;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SheddingController : ControllerBase
{
    private readonly ISheddingService _sheddingService;

    public SheddingController(ISheddingService sheddingService)
    {
        _sheddingService = sheddingService;
    }

    [HttpGet("reptile/{reptileId}")]
    public async Task<ActionResult<List<SheddingRecordDto>>> GetByReptileId(int reptileId)
    {
        return await _sheddingService.GetDtosByReptileIdAsync(reptileId);
    }

    [HttpGet("reptile/{reptileId}/last")]
    public async Task<ActionResult<DateTime?>> GetLastSheddingDate(int reptileId)
    {
        return await _sheddingService.GetLastSheddingDateAsync(reptileId);
    }
}