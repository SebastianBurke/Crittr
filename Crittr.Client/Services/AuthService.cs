using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text.Json;

namespace Crittr.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/login", new { Email = email, Password = password });

        if (response.IsSuccessStatusCode)
        {
            // Deserialize JSON without needing a full class
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var token = doc.RootElement.GetProperty("token").GetString();

            if (!string.IsNullOrEmpty(token))
            {
                await _localStorage.SetItemAsync("authToken", token);
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return true;
            }
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
    }
}