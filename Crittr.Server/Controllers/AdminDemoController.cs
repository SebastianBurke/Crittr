using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Crittr.Server.Data;
using Crittr.Server.Models;
using Crittr.Server.Services;
using System.Security.Cryptography;
using System.Text;

namespace Crittr.Server.Controllers;

[ApiController]
[Route("api/admin/demo")]
public class AdminDemoController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataSeeder _seeder;
    private readonly UserManager<AppUser> _userManager;
    private readonly DemoCredentials _demo;

    public AdminDemoController(
        IConfiguration config,
        DataSeeder seeder,
        UserManager<AppUser> userManager,
        DemoCredentials demo)
    {
        _config = config;
        _seeder = seeder;
        _userManager = userManager;
        _demo = demo;
    }

    [EnableRateLimiting("register")]
    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        if (!_config.GetValue<bool>("Demo:Enabled"))
            return NotFound();

        var configured = _config["Demo:AdminResetToken"];
        if (string.IsNullOrWhiteSpace(configured))
            return Unauthorized();

        if (!Request.Headers.TryGetValue("X-Admin-Token", out var supplied) || supplied.Count == 0)
            return Unauthorized();

        var a = Encoding.UTF8.GetBytes(configured);
        var b = Encoding.UTF8.GetBytes(supplied.ToString());
        if (a.Length != b.Length || !CryptographicOperations.FixedTimeEquals(a, b))
            return Unauthorized();

        var user = await _userManager.FindByEmailAsync(_demo.DemoEmail);
        if (user is null) return NotFound();

        await _seeder.ResetAsync(user.Id);

        return Ok(new { reset = true, at = DateTime.UtcNow });
    }
}
