using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crittr.Server.Services;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EnclosureController : ControllerBase
{
    private readonly IEnclosureService _enclosureService;

    public EnclosureController(IEnclosureService enclosureService)
    {
        _enclosureService = enclosureService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EnclosureProfile>>> GetAll()
    {
        return await _enclosureService.GetAllAsync();
    }

    [HttpGet("dto")]
    public async Task<ActionResult<List<EnclosureProfileDto>>> GetAllDtos()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        return await _enclosureService.GetAllDtosByUserIdAsync(userId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnclosureProfile>> GetById(int id)
    {
        var enclosure = await _enclosureService.GetByIdAsync(id);
        if (enclosure == null)
            return NotFound();

        return enclosure;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<EnclosureProfileDto>> GetDtoById(int id)
    {
        var dto = await _enclosureService.GetDtoByIdAsync(id);
        if (dto == null)
            return NotFound();

        return dto;
    }

    [HttpPost]
    public async Task<ActionResult<EnclosureProfile>> Create(EnclosureProfile enclosure)
    {
        var created = await _enclosureService.CreateAsync(enclosure);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("dto")]
    public async Task<ActionResult<EnclosureProfileDto>> CreateFromDto([FromBody] EnclosureProfileDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var model = new EnclosureProfile
        {
            Name = dto.Name,
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EnclosureProfile enclosure)
    {
        if (id != enclosure.Id)
            return BadRequest();

        var success = await _enclosureService.UpdateAsync(enclosure);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _enclosureService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
