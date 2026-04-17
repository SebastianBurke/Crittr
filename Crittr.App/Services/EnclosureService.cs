using System.Net.Http.Json;
using Crittr.Shared.DTOs;

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
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<EnclosureProfileDto>>() ?? new List<EnclosureProfileDto>();
    }

    public async Task<EnclosureProfileDto?> CreateEnclosureAsync(EnclosureProfileDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/enclosure/dto", dto);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<EnclosureProfileDto>();
    }
}