using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Crittr.Shared.DTOs;

public class CritterService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public CritterService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<List<CritterDto>> GetCrittersByEnclosureIdAsync(int enclosureId)
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var critters = await _http.GetFromJsonAsync<List<CritterDto>>($"api/critter/dto/by-enclosure/{enclosureId}");
            return critters ?? new List<CritterDto>();
        }
        catch (Exception ex)
        {
            return new List<CritterDto>();
        }
    }
    public async Task<List<CritterDto>> GetUnassignedCrittersAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var critters = await _http.GetFromJsonAsync<List<CritterDto>>($"api/critter/dto/unassigned/");
            return critters ?? new List<CritterDto>();
        }
        catch (Exception ex)
        {
            return new List<CritterDto>();
        }
    }
    
    public async Task<CritterDto?> CreateCritterAsync(CritterDto dto)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.PostAsJsonAsync("api/critter/dto", dto);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<CritterDto>();
    }
    
}