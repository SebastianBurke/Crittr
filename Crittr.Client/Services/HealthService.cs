using System.Net.Http.Json;
using Crittr.Shared.DTOs;

namespace Crittr.Client.Services;

public class HealthService
{
    private readonly HttpClient _http;

    public HealthService(HttpClient http) => _http = http;

    public async Task<List<HealthScoreDto>> GetByCritterIdAsync(int critterId)
        => await _http.GetFromJsonAsync<List<HealthScoreDto>>($"api/health/critter/{critterId}") ?? new();
}