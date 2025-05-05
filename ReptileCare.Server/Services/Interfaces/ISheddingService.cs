using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface ISheddingService
{
    Task<List<SheddingRecord>> GetAllAsync();
    Task<List<SheddingRecord>> GetByReptileIdAsync(int reptileId);
    Task<List<SheddingRecordDto>> GetDtosByReptileIdAsync(int reptileId);
    Task<SheddingRecord?> GetByIdAsync(int id);
    Task<SheddingRecordDto?> GetDtoByIdAsync(int id);
    Task<SheddingRecord> CreateAsync(SheddingRecord sheddingRecord);
    Task<bool> UpdateAsync(SheddingRecord sheddingRecord);
    Task<bool> DeleteAsync(int id);
    Task<DateTime?> GetLastSheddingDateAsync(int reptileId);
    Task<List<SheddingRecordDto>> GetIncompleteSheddingsAsync();
}
