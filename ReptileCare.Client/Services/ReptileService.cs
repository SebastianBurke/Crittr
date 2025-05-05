using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Client.Services;

public class ReptileService
{
    private readonly HttpClient _http;

    public ReptileService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ReptileDto>> GetReptilesAsync()
    {
        try
        {
            var reptiles = await _http.GetFromJsonAsync<List<ReptileDto>>("api/reptile/dto");
            return reptiles ?? new List<ReptileDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch reptiles: {ex.Message}");
            return new List<ReptileDto>();
        }
    }

}