using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services.Interfaces;

public interface IMeasurementService
{
    Task<List<MeasurementRecord>> GetAllAsync();
    Task<List<MeasurementRecord>> GetByReptileIdAsync(int reptileId);
    Task<List<MeasurementRecordDto>> GetDtosByReptileIdAsync(int reptileId);
    Task<MeasurementRecord?> GetByIdAsync(int id);
    Task<MeasurementRecordDto?> GetDtoByIdAsync(int id);
    Task<MeasurementRecord> CreateAsync(MeasurementRecord measurementRecord);
    Task<bool> UpdateAsync(MeasurementRecord measurementRecord);
    Task<bool> DeleteAsync(int id);
    Task<MeasurementRecordDto?> GetLatestForReptileAsync(int reptileId);
    Task<List<MeasurementRecordDto>> GetGrowthDataAsync(int reptileId, int months);
}
