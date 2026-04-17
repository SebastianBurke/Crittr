using System.Net.Http.Json;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.App.Services;

public class FeedingService
{
    private readonly HttpClient _http;

    public FeedingService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<FeedingRecordDto>> GetByCritterIdAsync(int critterId)
    {
        return await _http.GetFromJsonAsync<List<FeedingRecordDto>>($"api/feeding/critter/{critterId}/dto")
               ?? new();
    }

    public async Task<FeedingRecord?> CreateAsync(FeedingRecord record)
    {
        var response = await _http.PostAsJsonAsync("api/feeding", record);
        if (!response.IsSuccessStatusCode)
            return null;
        return await response.Content.ReadFromJsonAsync<FeedingRecord>();
    }

    public async Task<bool> DeleteAsync(int feedingRecordId)
    {
        var response = await _http.DeleteAsync($"api/feeding/{feedingRecordId}");
        return response.IsSuccessStatusCode;
    }
}
