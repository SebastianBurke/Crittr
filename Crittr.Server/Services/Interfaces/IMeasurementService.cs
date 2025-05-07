using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services.Interfaces;

public interface IMeasurementService
{
    Task<List<MeasurementRecord>> GetAllAsync();
    Task<List<MeasurementRecord>> GetByCritterIdAsync(int critterId);
    Task<List<MeasurementRecordDto>> GetDtosByCritterIdAsync(int critterId);
    Task<MeasurementRecord?> GetByIdAsync(int id);
    Task<MeasurementRecordDto?> GetDtoByIdAsync(int id);
    Task<MeasurementRecord> CreateAsync(MeasurementRecord measurementRecord);
    Task<bool> UpdateAsync(MeasurementRecord measurementRecord);
    Task<bool> DeleteAsync(int id);
    Task<MeasurementRecordDto?> GetLatestForCritterAsync(int critterId);
    Task<List<MeasurementRecordDto>> GetGrowthDataAsync(int critterId, int months);
}
