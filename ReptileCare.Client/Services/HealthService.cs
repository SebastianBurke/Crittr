using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Client.Services;

public class HealthService
{
    private readonly HttpClient _http;

    public HealthService(HttpClient http) => _http = http;

    public async Task<List<HealthScoreDto>> GetByReptileIdAsync(int reptileId)
        => await _http.GetFromJsonAsync<List<HealthScoreDto>>($"api/health/reptile/{reptileId}") ?? new();
}