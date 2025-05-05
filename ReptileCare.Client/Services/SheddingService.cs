using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Client.Services;

public class SheddingService
{
    private readonly HttpClient _http;

    public SheddingService(HttpClient http) => _http = http;

    public async Task<List<SheddingRecordDto>> GetByReptileIdAsync(int reptileId)
        => await _http.GetFromJsonAsync<List<SheddingRecordDto>>($"api/shedding/reptile/{reptileId}") ?? new();
}