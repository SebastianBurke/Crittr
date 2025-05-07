using System.Net.Http.Json;
using Crittr.Shared.DTOs;

namespace Crittr.Client.Services;

public class FeedingService
{
    private readonly HttpClient _http;

    public FeedingService(HttpClient http) => _http = http;

    public async Task<List<FeedingRecordDto>> GetByCritterIdAsync(int critterId)
        => await _http.GetFromJsonAsync<List<FeedingRecordDto>>($"api/feeding/critter/{critterId}") ?? new();
}