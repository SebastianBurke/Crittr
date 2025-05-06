using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;

public class ReptileService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public ReptileService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
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