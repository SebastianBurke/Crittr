using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services;

public class SheddingService : ISheddingService
{
    private readonly ApplicationDbContext _db;

    public SheddingService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<SheddingRecord>> GetAllAsync()
    {
        return await _db.SheddingRecords
            .Include(s => s.Critter)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<List<SheddingRecord>> GetByCritterIdAsync(int critterId)
    {
        return await _db.SheddingRecords
            .Include(s => s.Critter)
            .Where(s => s.CritterId == critterId)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<List<SheddingRecordDto>> GetDtosByCritterIdAsync(int critterId)
    {
        var records = await GetByCritterIdAsync(critterId);
        return records.Select(CreateDtoFromRecord).ToList();
    }

    public async Task<SheddingRecord?> GetByIdAsync(int id)
    {
        return await _db.SheddingRecords
            .Include(s => s.Critter)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<SheddingRecordDto?> GetDtoByIdAsync(int id)
    {
        var record = await GetByIdAsync(id);
        return record != null ? CreateDtoFromRecord(record) : null;
    }

    public async Task<SheddingRecord> CreateAsync(SheddingRecord sheddingRecord)
    {
        await _db.SheddingRecords.AddAsync(sheddingRecord);
        await _db.SaveChangesAsync();
        return sheddingRecord;
    }

    public async Task<bool> UpdateAsync(SheddingRecord sheddingRecord)
    {
        var existing = await _db.SheddingRecords.FindAsync(sheddingRecord.Id);
        if (existing == null)
            return false;

        _db.Entry(existing).CurrentValues.SetValues(sheddingRecord);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var record = await _db.SheddingRecords.FindAsync(id);
        if (record == null)
            return false;

        _db.SheddingRecords.Remove(record);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<DateTime?> GetLastSheddingDateAsync(int critterId)
    {
        var last = await _db.SheddingRecords
            .Where(s => s.CritterId == critterId && s.IsComplete && s.CompletionDate != null)
            .OrderByDescending(s => s.CompletionDate)
            .FirstOrDefaultAsync();

        return last?.CompletionDate;
    }

    public async Task<List<SheddingRecordDto>> GetIncompleteSheddingsAsync()
    {
        var incomplete = await _db.SheddingRecords
            .Include(s => s.Critter)
            .Where(s => !s.IsComplete)
            .ToListAsync();

        return incomplete.Select(CreateDtoFromRecord).ToList();
    }

    private SheddingRecordDto CreateDtoFromRecord(SheddingRecord record)
    {
        return new SheddingRecordDto
        {
            Id = record.Id,
            CritterId = record.CritterId,
            CritterName = record.Critter?.Name ?? string.Empty,
            StartDate = record.StartDate,
            CompletionDate = record.CompletionDate,
            IsComplete = record.IsComplete,
            WasAssisted = record.WasAssisted,
            Notes = record.Notes
        };
    }
}
