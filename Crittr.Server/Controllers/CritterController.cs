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
using Crittr.Shared.Utilities;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CritterController : ControllerBase
{
    private readonly ICritterService _critterService;
    private readonly ApplicationDbContext _db;
    private readonly EnclosureCohabitationService _cohabitation;

    public CritterController(ICritterService critterService, ApplicationDbContext db, EnclosureCohabitationService cohabitation)
    {
        _critterService = critterService;
        _db = db;
        _cohabitation = cohabitation;
    }

    private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private async Task<bool> OwnsEnclosureAsync(int enclosureId)
    {
        var uid = CurrentUserId;
        if (uid is null) return false;
        return await _db.EnclosureProfiles.AnyAsync(e => e.Id == enclosureId && e.OwnerId == uid);
    }

    [HttpGet]
    public async Task<ActionResult<List<Critter>>> GetAll()
    {
        var uid = CurrentUserId;
        if (uid is null) return Unauthorized();
        return await _critterService.GetAllByUserAsync(uid);
    }

    [HttpGet("dto/by-enclosure/{enclosureId}")]
    public async Task<ActionResult<List<CritterDto>>> GetAllByEnclosureId(int enclosureId)
    {
        if (!await OwnsEnclosureAsync(enclosureId))
            return Forbid();

        var critters = await _critterService.GetAllDtosByEnclosureIdAsync(enclosureId);
        return Ok(critters);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Critter>> GetById(int id)
    {
        var critter = await _critterService.GetByIdAsync(id);
        if (critter == null)
            return NotFound();

        if (critter.UserId != CurrentUserId)
            return Forbid();

        return critter;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<CritterDto>> GetDtoById(int id)
    {
        var critterDto = await _critterService.GetDtoByIdAsync(id);
        if (critterDto == null)
            return NotFound();

        if (critterDto.UserId != CurrentUserId)
            return Forbid();

        return critterDto;
    }

    [HttpGet("dto/unassigned")]
    public async Task<ActionResult<List<CritterDto>>> GetUnassignedCritters()
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var critters = await _critterService.GetUnassignedCrittersByUserAsync(userId);
            return Ok(critters);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error getting unassigned critters: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Critter>> Create(Critter critter)
    {
        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();

        critter.UserId = userId;
        critter.Id = 0;

        var createdCritter = await _critterService.CreateAsync(critter);
        return CreatedAtAction(nameof(GetById), new { id = createdCritter.Id }, createdCritter);
    }

    [HttpPost("dto")]
    public async Task<ActionResult<CritterDto>> CreateFromDto([FromBody] CritterDto dto, [FromQuery] bool force = false)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        if (!force && dto.EnclosureProfileId.HasValue)
        {
            var enclosure = await _db.EnclosureProfiles.FindAsync(dto.EnclosureProfileId.Value);
            if (enclosure != null && !EnclosureCompatibility.IsCompatible(dto.SpeciesType, enclosure.EnclosureType))
            {
                var ideal = EnclosureCompatibility.FormatEnclosureType(EnclosureCompatibility.GetIdealEnclosureType(dto.SpeciesType));
                var actual = EnclosureCompatibility.FormatEnclosureType(enclosure.EnclosureType);
                return UnprocessableEntity(new { error = "incompatible", message = $"A {dto.SpeciesType} is not suited for a {actual}. Consider {EnclosureCompatibility.GetEnclosureRequirementLabel(dto.SpeciesType)} (e.g. a {ideal}). Use ?force=true to override." });
            }
        }

        var critter = new Critter
        {
            Name = dto.Name,
            Species = dto.Species,
            SpeciesType = dto.SpeciesType,
            DateOfBirth = dto.DateOfBirth,
            DateAcquired = dto.DateAcquired,
            Description = dto.Description,
            Sex = dto.Sex,
            EnclosureProfileId = dto.EnclosureProfileId,
            IconUrl = dto.IconUrl,
            UserId = userId
        };

        await using var tx = await _db.Database.BeginTransactionAsync();
        var created = await _critterService.CreateAsync(critter);

        if (!force && dto.EnclosureProfileId.HasValue)
        {
            var cohabResult = await _cohabitation.CheckAsync(created.Id, dto.EnclosureProfileId.Value, userId);
            if (!cohabResult.CanCohabit)
            {
                await tx.RollbackAsync();
                return UnprocessableEntity(new { error = "cohab_blocked", conflicts = cohabResult.Conflicts });
            }
        }

        await tx.CommitAsync();
        var dtoResult = await _critterService.GetDtoByIdAsync(created.Id);

        return CreatedAtAction(nameof(GetDtoById), new { id = created.Id }, dtoResult);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Critter critter)
    {
        if (id != critter.Id)
            return BadRequest();

        var existing = await _critterService.GetByIdAsync(id);
        if (existing is null)
            return NotFound();
        if (existing.UserId != CurrentUserId)
            return Forbid();

        critter.UserId = existing.UserId;

        var success = await _critterService.UpdateAsync(critter);
        if (!success)
            return NotFound();

        return NoContent();
    }

    public record AssignEnclosureRequest(int? EnclosureId);

    [HttpPatch("{id}/enclosure")]
    public async Task<IActionResult> AssignEnclosure(int id, [FromBody] AssignEnclosureRequest request, [FromQuery] bool force = false)
    {
        var existing = await _critterService.GetByIdAsync(id);
        if (existing is null) return NotFound();
        var userId = CurrentUserId;
        if (existing.UserId != userId) return Forbid();

        if (request.EnclosureId.HasValue)
        {
            if (!await OwnsEnclosureAsync(request.EnclosureId.Value))
                return Forbid();

            if (!force)
            {
                var enclosure = await _db.EnclosureProfiles.FindAsync(request.EnclosureId.Value);
                if (enclosure != null && !EnclosureCompatibility.IsCompatible(existing.SpeciesType, enclosure.EnclosureType))
                {
                    var ideal = EnclosureCompatibility.FormatEnclosureType(EnclosureCompatibility.GetIdealEnclosureType(existing.SpeciesType));
                    var actual = EnclosureCompatibility.FormatEnclosureType(enclosure.EnclosureType);
                    return UnprocessableEntity(new { error = "incompatible", message = $"A {existing.SpeciesType} is not suited for a {actual}. Consider {EnclosureCompatibility.GetEnclosureRequirementLabel(existing.SpeciesType)} (e.g. a {ideal}). Use ?force=true to override." });
                }

                var cohabResult = await _cohabitation.CheckAsync(id, request.EnclosureId.Value, userId!);
                if (!cohabResult.CanCohabit)
                    return UnprocessableEntity(new { error = "cohab_blocked", conflicts = cohabResult.Conflicts });
            }
        }

        existing.EnclosureProfileId = request.EnclosureId;
        await _critterService.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _critterService.GetByIdAsync(id);
        if (existing is null)
            return NotFound();
        if (existing.UserId != CurrentUserId)
            return Forbid();

        var success = await _critterService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<List<CritterDto>>> Search(string searchTerm)
    {
        var uid = CurrentUserId;
        if (uid is null) return Unauthorized();
        return await _critterService.SearchByUserAsync(searchTerm, uid);
    }
}