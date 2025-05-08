using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CritterController : ControllerBase
{
    private readonly ICritterService _critterService;

    public CritterController(ICritterService critterService)
    {
        _critterService = critterService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Critter>>> GetAll()
    {
        return await _critterService.GetAllAsync();
    }

    [HttpGet("dto/by-enclosure/{enclosureId}")]
    public async Task<ActionResult<List<CritterDto>>> GetAllByEnclosureId(int enclosureId)
    {
        var critters = await _critterService.GetAllDtosByEnclosureIdAsync(enclosureId);
        return Ok(critters);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Critter>> GetById(int id)
    {
        var critter = await _critterService.GetByIdAsync(id);
        if (critter == null)
            return NotFound();

        return critter;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<CritterDto>> GetDtoById(int id)
    {
        var critterDto = await _critterService.GetDtoByIdAsync(id);
        if (critterDto == null)
            return NotFound();

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
        var createdCritter = await _critterService.CreateAsync(critter);
        return CreatedAtAction(nameof(GetById), new { id = createdCritter.Id }, createdCritter);
    }

    [HttpPost("dto")]
    public async Task<ActionResult<CritterDto>> CreateFromDto([FromBody] CritterDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var critter = new Critter
        {
            Name = dto.Name,
            Species = dto.Species,
            DateOfBirth = dto.DateOfBirth,
            DateAcquired = dto.DateAcquired,
            Description = dto.Description,
            Sex = dto.Sex,
            EnclosureProfileId = dto.EnclosureProfileId,
            IconUrl = dto.IconUrl,
            UserId = userId
        };

        var created = await _critterService.CreateAsync(critter);
        var dtoResult = await _critterService.GetDtoByIdAsync(created.Id);

        return CreatedAtAction(nameof(GetDtoById), new { id = created.Id }, dtoResult);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Critter critter)
    {
        if (id != critter.Id)
            return BadRequest();

        var success = await _critterService.UpdateAsync(critter);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _critterService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<List<CritterDto>>> Search(string searchTerm)
    {
        return await _critterService.SearchAsync(searchTerm);
    }
}