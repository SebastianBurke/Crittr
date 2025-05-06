using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ReptileController : ControllerBase
{
    private readonly IReptileService _reptileService;

    public ReptileController(IReptileService reptileService)
    {
        _reptileService = reptileService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Reptile>>> GetAll()
    {
        return await _reptileService.GetAllAsync();
    }

    [HttpGet("dto")]
    public async Task<ActionResult<List<ReptileDto>>> GetAllDtos()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        return await _reptileService.GetAllDtosByUserIdAsync(userId);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Reptile>> GetById(int id)
    {
        var reptile = await _reptileService.GetByIdAsync(id);
        if (reptile == null)
            return NotFound();

        return reptile;
    }

    [HttpGet("dto/{id}")]
    public async Task<ActionResult<ReptileDto>> GetDtoById(int id)
    {
        var reptileDto = await _reptileService.GetDtoByIdAsync(id);
        if (reptileDto == null)
            return NotFound();

        return reptileDto;
    }

    [HttpPost]
    public async Task<ActionResult<Reptile>> Create(Reptile reptile)
    {
        var createdReptile = await _reptileService.CreateAsync(reptile);
        return CreatedAtAction(nameof(GetById), new { id = createdReptile.Id }, createdReptile);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Reptile reptile)
    {
        if (id != reptile.Id)
            return BadRequest();

        var success = await _reptileService.UpdateAsync(reptile);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _reptileService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<List<ReptileDto>>> Search(string searchTerm)
    {
        return await _reptileService.SearchAsync(searchTerm);
    }
}
