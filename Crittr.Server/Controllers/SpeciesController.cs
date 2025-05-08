using Crittr.Server.Services;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeciesController : ControllerBase
{
    private readonly SpeciesCatalogService _catalog;

    public SpeciesController(SpeciesCatalogService catalog)
    {
        _catalog = catalog;
    }

    [HttpGet]
    public async Task<ActionResult<List<SpeciesInfo>>> GetAll()
    {
        return await _catalog.GetAllAsync();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<SpeciesInfo>>> Search([FromQuery] string q, [FromQuery] SpeciesType? type)
    {
        var results = await _catalog.SearchAsync(q, type);
        return results;
    }
}