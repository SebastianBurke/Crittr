using System.Net.Http.Json;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

public class SpeciesService
{
    private readonly HttpClient _http;

    public SpeciesService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SpeciesInfo>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<SpeciesInfo>>("api/species") ?? new();
    }

    public async Task<List<SpeciesInfo>> SearchAsync(string query, SpeciesType? type = null)
    {
        var url = $"api/species/search?q={Uri.EscapeDataString(query)}";

        if (type.HasValue)
            url += $"&type={(int)type.Value}";

        return await _http.GetFromJsonAsync<List<SpeciesInfo>>(url) ?? new();
    }
}