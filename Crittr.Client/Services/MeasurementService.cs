using System.Net.Http.Json;
using Crittr.Shared.DTOs;

namespace Crittr.Client.Services;

public class MeasurementService
{
    private readonly HttpClient _http;

    public MeasurementService(HttpClient http) => _http = http;

    public async Task<List<MeasurementRecordDto>> GetByCritterIdAsync(int critterId)
        => await _http.GetFromJsonAsync<List<MeasurementRecordDto>>($"api/measurement/critter/{critterId}") ?? new();
}