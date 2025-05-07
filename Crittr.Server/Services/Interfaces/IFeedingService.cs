using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface IFeedingService
{
    Task<List<FeedingRecord>> GetAllAsync();
    Task<List<FeedingRecord>> GetByCritterIdAsync(int critterId);
    Task<List<FeedingRecordDto>> GetDtosByCritterIdAsync(int critterId);
    Task<FeedingRecord?> GetByIdAsync(int id);
    Task<FeedingRecordDto?> GetDtoByIdAsync(int id);
    Task<FeedingRecord> CreateAsync(FeedingRecord feedingRecord);
    Task<bool> UpdateAsync(FeedingRecord feedingRecord);
    Task<bool> DeleteAsync(int id);
    Task<DateTime?> GetLastFeedingDateAsync(int critterId);
}