using System.Net.Http.Json;
using Crittr.Shared.DTOs;

namespace Crittr.Client.Services;

public class SheddingService
{
    private readonly HttpClient _http;

    public SheddingService(HttpClient http) => _http = http;

    public async Task<List<SheddingRecordDto>> GetByCritterIdAsync(int critterId)
        => await _http.GetFromJsonAsync<List<SheddingRecordDto>>($"api/shedding/critter/{critterId}") ?? new();
}