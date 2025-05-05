using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IFeedingService
{
    Task<List<FeedingRecord>> GetAllAsync();
    Task<List<FeedingRecord>> GetByReptileIdAsync(int reptileId);
    Task<List<FeedingRecordDto>> GetDtosByReptileIdAsync(int reptileId);
    Task<FeedingRecord?> GetByIdAsync(int id);
    Task<FeedingRecordDto?> GetDtoByIdAsync(int id);
    Task<FeedingRecord> CreateAsync(FeedingRecord feedingRecord);
    Task<bool> UpdateAsync(FeedingRecord feedingRecord);
    Task<bool> DeleteAsync(int id);
    Task<DateTime?> GetLastFeedingDateAsync(int reptileId);
}