using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Data;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services;

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
            .Include(s => s.Reptile)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<List<SheddingRecord>> GetByReptileIdAsync(int reptileId)
    {
        return await _db.SheddingRecords
            .Include(s => s.Reptile)
            .Where(s => s.ReptileId == reptileId)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<List<SheddingRecordDto>> GetDtosByReptileIdAsync(int reptileId)
    {
        var records = await GetByReptileIdAsync(reptileId);
        return records.Select(CreateDtoFromRecord).ToList();
    }

    public async Task<SheddingRecord?> GetByIdAsync(int id)
    {
        return await _db.SheddingRecords
            .Include(s => s.Reptile)
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

    public async Task<DateTime?> GetLastSheddingDateAsync(int reptileId)
    {
        var last = await _db.SheddingRecords
            .Where(s => s.ReptileId == reptileId && s.IsComplete && s.CompletionDate != null)
            .OrderByDescending(s => s.CompletionDate)
            .FirstOrDefaultAsync();

        return last?.CompletionDate;
    }

    public async Task<List<SheddingRecordDto>> GetIncompleteSheddingsAsync()
    {
        var incomplete = await _db.SheddingRecords
            .Include(s => s.Reptile)
            .Where(s => !s.IsComplete)
            .ToListAsync();

        return incomplete.Select(CreateDtoFromRecord).ToList();
    }

    private SheddingRecordDto CreateDtoFromRecord(SheddingRecord record)
    {
        return new SheddingRecordDto
        {
            Id = record.Id,
            ReptileId = record.ReptileId,
            ReptileName = record.Reptile?.Name ?? string.Empty,
            StartDate = record.StartDate,
            CompletionDate = record.CompletionDate,
            IsComplete = record.IsComplete,
            WasAssisted = record.WasAssisted,
            Notes = record.Notes
        };
    }
}
