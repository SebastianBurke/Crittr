using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Client.Services;

public class FeedingService
{
    private readonly HttpClient _http;

    public FeedingService(HttpClient http) => _http = http;

    public async Task<List<FeedingRecordDto>> GetByReptileIdAsync(int reptileId)
        => await _http.GetFromJsonAsync<List<FeedingRecordDto>>($"api/feeding/reptile/{reptileId}") ?? new();
}