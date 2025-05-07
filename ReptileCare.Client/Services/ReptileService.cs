using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;

public class ReptileService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public ReptileService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<List<ReptileDto>> GetReptilesByEnclosureIdAsync(int enclosureId)
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var reptiles = await _http.GetFromJsonAsync<List<ReptileDto>>($"api/reptile/dto/by-enclosure/{enclosureId}");
            return reptiles ?? new List<ReptileDto>();
        }
        catch (Exception ex)
        {
            return new List<ReptileDto>();
        }
    }
    
    public async Task<ReptileDto?> CreateReptileAsync(ReptileDto dto)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.PostAsJsonAsync("api/reptile/dto", dto);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<ReptileDto>();
    }
}