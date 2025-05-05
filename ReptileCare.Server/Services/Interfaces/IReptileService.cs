using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IReptileService
{
    Task<List<Reptile>> GetAllAsync();
}