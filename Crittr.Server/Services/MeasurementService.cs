using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services;

public class MeasurementService : IMeasurementService
{
    private readonly ApplicationDbContext _db;

    public MeasurementService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<MeasurementRecord>> GetAllAsync()
    {
        return await _db.MeasurementRecords
            .Include(m => m.Critter)
            .OrderByDescending(m => m.MeasurementDate)
            .ToListAsync();
    }

    public async Task<List<MeasurementRecord>> GetByCritterIdAsync(int critterId)
    {
        return await _db.MeasurementRecords
            .Include(m => m.Critter)
            .Where(m => m.CritterId == critterId)
            .OrderBy(m => m.MeasurementDate)
            .ToListAsync();
    }

    public async Task<List<MeasurementRecordDto>> GetDtosByCritterIdAsync(int critterId)
    {
        var records = await GetByCritterIdAsync(critterId);
        return CreateDtosWithChange(records);
    }

    public async Task<MeasurementRecord?> GetByIdAsync(int id)
    {
        return await _db.MeasurementRecords
            .Include(m => m.Critter)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<MeasurementRecordDto?> GetDtoByIdAsync(int id)
    {
        var record = await GetByIdAsync(id);
        if (record == null) return null;

        var previous = await _db.MeasurementRecords
            .Where(m => m.CritterId == record.CritterId && m.MeasurementDate < record.MeasurementDate)
            .OrderByDescending(m => m.MeasurementDate)
            .FirstOrDefaultAsync();

        return CreateDtoFromRecord(record, previous);
    }

    public async Task<MeasurementRecord> CreateAsync(MeasurementRecord measurementRecord)
    {
        await _db.MeasurementRecords.AddAsync(measurementRecord);
        await _db.SaveChangesAsync();
        return measurementRecord;
    }

    public async Task<bool> UpdateAsync(MeasurementRecord measurementRecord)
    {
        var existing = await _db.MeasurementRecords.FindAsync(measurementRecord.Id);
        if (existing == null) return false;

        _db.Entry(existing).CurrentValues.SetValues(measurementRecord);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var record = await _db.MeasurementRecords.FindAsync(id);
        if (record == null) return false;

        _db.MeasurementRecords.Remove(record);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<MeasurementRecordDto?> GetLatestForCritterAsync(int critterId)
    {
        var latest = await _db.MeasurementRecords
            .Include(m => m.Critter)
            .Where(m => m.CritterId == critterId)
            .OrderByDescending(m => m.MeasurementDate)
            .FirstOrDefaultAsync();

        if (latest == null) return null;

        var previous = await _db.MeasurementRecords
            .Where(m => m.CritterId == critterId && m.MeasurementDate < latest.MeasurementDate)
            .OrderByDescending(m => m.MeasurementDate)
            .FirstOrDefaultAsync();

        return CreateDtoFromRecord(latest, previous);
    }

    public async Task<List<MeasurementRecordDto>> GetGrowthDataAsync(int critterId, int months)
    {
        var cutoff = DateTime.UtcNow.AddMonths(-months);
        var records = await _db.MeasurementRecords
            .Include(m => m.Critter)
            .Where(m => m.CritterId == critterId && m.MeasurementDate >= cutoff)
            .OrderBy(m => m.MeasurementDate)
            .ToListAsync();

        return CreateDtosWithChange(records);
    }

    // ------------------------- Helpers ----------------------------

    private List<MeasurementRecordDto> CreateDtosWithChange(List<MeasurementRecord> records)
    {
        var dtos = new List<MeasurementRecordDto>();
        MeasurementRecord? prev = null;

        foreach (var record in records)
        {
            var dto = CreateDtoFromRecord(record, prev);
            dtos.Add(dto);
            prev = record;
        }

        return dtos;
    }

    private MeasurementRecordDto CreateDtoFromRecord(MeasurementRecord current, MeasurementRecord? previous)
    {
        return new MeasurementRecordDto
        {
            Id = current.Id,
            CritterId = current.CritterId,
            CritterName = current.Critter?.Name ?? string.Empty,
            MeasurementDate = current.MeasurementDate,
            Weight = current.Weight,
            Length = current.Length,
            Notes = current.Notes,
            WeightChange = previous != null ? current.Weight - previous.Weight : null,
            LengthChange = previous != null ? current.Length - previous.Length : null
        };
    }
}
