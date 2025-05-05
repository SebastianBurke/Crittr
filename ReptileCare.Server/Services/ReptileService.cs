using ReptileCare.Shared.Data;
using ReptileCare.Shared.Models;
using ReptileCare.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReptileCare.Server.Services;

public class ReptileService : IReptileService
{
    private readonly ApplicationDbContext _db;

    public ReptileService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Reptile>> GetAllAsync()
    {
        return await _db.Reptiles.ToListAsync();
    }

    // You can add more CRUD methods here as needed
}