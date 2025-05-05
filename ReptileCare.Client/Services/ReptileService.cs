using System.Net.Http.Json;
using ReptileCare.Shared.Models;

namespace ReptileCare.Client.Services;

public class ReptileService
{
    private readonly HttpClient _http;

    public ReptileService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Reptile>> GetReptilesAsync()
    {
        try
        {
            var reptiles = await _http.GetFromJsonAsync<List<Reptile>>("api/reptile");
            return reptiles ?? new List<Reptile>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch reptiles: {ex.Message}");
            return new List<Reptile>();
        }
    }

}