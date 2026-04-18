using System.Net.Http.Json;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

namespace Crittr.App.Services;

public class EnclosureService
{
    private readonly HttpClient _http;

    public EnclosureService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<EnclosureProfileDto>> GetEnclosuresAsync()
    {
        var response = await _http.GetAsync("api/enclosure/dto");
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException("User is not authenticated.");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<EnclosureProfileDto>>() ?? [];
    }

    public async Task<EnclosureProfileDto?> GetEnclosureByIdAsync(int id)
    {
        var response = await _http.GetAsync($"api/enclosure/dto/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<EnclosureProfileDto>();
    }

    public async Task<List<EnclosureProfileDto>> GetCompatibleEnclosuresAsync(SpeciesType speciesType)
    {
        var response = await _http.GetAsync($"api/enclosure/dto/compatible?speciesType={(int)speciesType}");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<EnclosureProfileDto>>() ?? [];
    }

    public async Task<EnclosureProfileDto?> CreateEnclosureAsync(EnclosureProfileDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/enclosure/dto", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<EnclosureProfileDto>();
    }

    public async Task<bool> UpdateEnclosureAsync(EnclosureProfileDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/enclosure/dto/{dto.Id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteEnclosureAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/enclosure/{id}");
        return response.IsSuccessStatusCode;
    }
}