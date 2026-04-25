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
public class SheddingController : ControllerBase
{
    private readonly ISheddingService _sheddingService;
    private readonly ApplicationDbContext _db;

    public SheddingController(ISheddingService sheddingService, ApplicationDbContext db)
    {
        _sheddingService = sheddingService;
        _db = db;
    }

    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private async Task<bool> OwnsCritterAsync(int critterId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.Critters.AnyAsync(c => c.Id == critterId && c.UserId == uid);
    }

    [HttpGet("critter/{critterId}")]
    public async Task<ActionResult<List<SheddingRecordDto>>> GetByCritterId(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _sheddingService.GetDtosByCritterIdAsync(critterId);
    }

    [HttpGet("critter/{critterId}/last")]
    public async Task<ActionResult<DateTime?>> GetLastSheddingDate(int critterId)
    {
        if (!await OwnsCritterAsync(critterId))
            return Forbid();

        return await _sheddingService.GetLastSheddingDateAsync(critterId);
    }
}
