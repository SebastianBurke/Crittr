using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Client.Services;

public class MeasurementService
{
    private readonly HttpClient _http;

    public MeasurementService(HttpClient http) => _http = http;

    public async Task<List<MeasurementRecordDto>> GetByReptileIdAsync(int reptileId)
        => await _http.GetFromJsonAsync<List<MeasurementRecordDto>>($"api/measurement/reptile/{reptileId}") ?? new();
}