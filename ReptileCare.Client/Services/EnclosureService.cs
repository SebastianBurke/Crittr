using System.Net.Http.Json;
using Blazored.LocalStorage;
using ReptileCare.Shared.DTOs;

public class EnclosureService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public EnclosureService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<List<EnclosureProfileDto>> GetEnclosuresAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.GetAsync("api/enclosure/dto");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<EnclosureProfileDto>>() ?? new List<EnclosureProfileDto>();
    }
}