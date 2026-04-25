using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;
using Crittr.Shared.Models.Enums;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EnclosureController : ControllerBase
{
    private readonly IEnclosureService _enclosureService;
    private readonly EnclosureCohabitationService _cohabitation;
    private readonly ApplicationDbContext _db;

    public EnclosureController(IEnclosureService enclosureService, EnclosureCohabitationService cohabitation, ApplicationDbContext db)
    {
        _enclosureService = enclosureService;
        _cohabitation = cohabitation;
        _db = db;
    }

    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private async Task<bool> OwnsEnclosureAsync(int enclosureId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.EnclosureProfiles.AnyAsync(e => e.Id == enclosureId && e.OwnerId == uid);
    }

    [HttpGet("{enclosureId}/cohabitation")]
    public async Task<ActionResult<CohabitationCheckResultDto>> CheckCohabitation(
        int enclosureId, [FromQuery] int critterId)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();
        if (!await OwnsEnclosureAsync(enclosureId)) return Forbid();
        return await _cohabitation.CheckAsync(critterId, enclosureId, userId);
    }

    [HttpGet]
    public async Task<ActionResult<List<EnclosureProfileDto>>> GetAll([FromQuery] int take = 100, [FromQuery] int skip = 0)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        return await _enclosureService.GetAllDtosByUserIdAsync(userId, take, skip);
    }

    [HttpGet("dto")]
    public async Task<ActionResult<List<EnclosureProfileDto>>> GetAllDtos([FromQuery] int take = 100, [FromQuery] int skip = 0)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        return await _enclosureService.GetAllDtosByUserIdAsync(userId, take, skip);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnclosureProfile>> GetById(int id)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        var enclosure = await _enclosureService.GetByIdAsync(id);
        if (enclosure == null)
            return NotFound();

        if (enclosure.OwnerId != userId)
            return Forbid();

        return enclosure;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<EnclosureProfileDto>> GetDtoById(int id)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        var dto = await _enclosureService.GetDtoByIdAsync(id, userId);
        if (dto == null)
        {
            var exists = await _db.EnclosureProfiles.AnyAsync(e => e.Id == id);
            return exists ? Forbid() : NotFound();
        }

        return dto;
    }

    [HttpPost("dto")]
    public async Task<ActionResult<EnclosureProfileDto>> CreateFromDto([FromBody] EnclosureProfileDto dto)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        var model = new EnclosureProfile
        {
            Name = dto.Name,
            EnclosureType = dto.EnclosureType,
            SubstrateType = dto.SubstrateType,
            Length = dto.Length,
            Width = dto.Width,
            Height = dto.Height,
            HasUVBLighting = dto.HasUVBLighting,
            HasHeatingElement = dto.HasHeatingElement,
            IdealTemperature = dto.IdealTemperature,
            IdealHumidity = dto.IdealHumidity,
            OwnerId = userId
        };

        var created = await _enclosureService.CreateAsync(model);
        return CreatedAtAction(nameof(GetDtoById), new { id = created.Id }, EnclosureService.MapToDto(created));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        var success = await _enclosureService.DeleteAsync(id, userId);
        if (!success)
        {
            var exists = await _db.EnclosureProfiles.AnyAsync(e => e.Id == id);
            return exists ? Forbid() : NotFound();
        }

        return NoContent();
    }

    [HttpGet("dto/compatible")]
    public async Task<ActionResult<List<EnclosureProfileDto>>> GetCompatible([FromQuery] SpeciesType speciesType)
    {
        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        return await _enclosureService.GetCompatibleByUserIdAsync(userId, speciesType);
    }

    [HttpPut("dto/{id}")]
    public async Task<IActionResult> UpdateFromDto(int id, [FromBody] EnclosureProfileDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var userId = CurrentUserId;
        if (userId == null) return Unauthorized();

        var success = await _enclosureService.UpdateFromDtoAsync(dto, userId);
        if (!success)
        {
            var exists = await _db.EnclosureProfiles.AnyAsync(e => e.Id == id);
            return exists ? Forbid() : NotFound();
        }

        return NoContent();
    }
}
