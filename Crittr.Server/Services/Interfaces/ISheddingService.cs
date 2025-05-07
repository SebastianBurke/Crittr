using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface ISheddingService
{
    Task<List<SheddingRecord>> GetAllAsync();
    Task<List<SheddingRecord>> GetByCritterIdAsync(int critterId);
    Task<List<SheddingRecordDto>> GetDtosByCritterIdAsync(int critterId);
    Task<SheddingRecord?> GetByIdAsync(int id);
    Task<SheddingRecordDto?> GetDtoByIdAsync(int id);
    Task<SheddingRecord> CreateAsync(SheddingRecord sheddingRecord);
    Task<bool> UpdateAsync(SheddingRecord sheddingRecord);
    Task<bool> DeleteAsync(int id);
    Task<DateTime?> GetLastSheddingDateAsync(int critterId);
    Task<List<SheddingRecordDto>> GetIncompleteSheddingsAsync();
}
