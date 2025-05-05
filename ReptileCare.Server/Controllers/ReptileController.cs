using Microsoft.AspNetCore.Mvc;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReptileController : ControllerBase
{
    private readonly IReptileService _reptileService;

    public ReptileController(IReptileService reptileService)
    {
        _reptileService = reptileService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Reptile>>> Get()
    {
        return await _reptileService.GetAllAsync();
    }
}
