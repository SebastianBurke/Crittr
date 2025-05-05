using System.Net.Http.Json;
using ReptileCare.Shared.DTOs;

namespace ReptileCare.Client.Services;

public class ScheduledTaskService
{
    private readonly HttpClient _http;

    public ScheduledTaskService(HttpClient http) => _http = http;

    public async Task<List<ScheduledTaskDto>> GetByReptileIdAsync(int reptileId)
        => await _http.GetFromJsonAsync<List<ScheduledTaskDto>>($"api/tasks/reptile/{reptileId}") ?? new();

    public async Task<List<ScheduledTaskDto>> GetUpcomingAsync(int days = 7)
        => await _http.GetFromJsonAsync<List<ScheduledTaskDto>>($"api/tasks/upcoming/{days}") ?? new();

    public async Task<List<ScheduledTaskDto>> GetOverdueAsync()
        => await _http.GetFromJsonAsync<List<ScheduledTaskDto>>("api/tasks/overdue") ?? new();
}