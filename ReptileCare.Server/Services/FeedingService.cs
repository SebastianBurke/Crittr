namespace ReptileCare.Server.Services;


// ReptileCare.Server/Services/FeedingService.cs
using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.Data;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

public class FeedingService : IFeedingService
{
    private readonly ApplicationDbContext _db;

    public FeedingService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<FeedingRecord>> GetAllAsync()
    {
        return await _db.FeedingRecords
            .Include(f => f.Reptile)
            .OrderByDescending(f => f.FeedingDate)
            .ToListAsync();
    }

    public async Task<List<FeedingRecord>> GetByReptileIdAsync(int reptileId)
    {
        return await _db.FeedingRecords
            .Include(f => f.Reptile)
            .Where(f => f.ReptileId == reptileId)
            .OrderByDescending(f => f.FeedingDate)
            .ToListAsync();
    }

    public async Task<List<FeedingRecordDto>> GetDtosByReptileIdAsync(int reptileId)
    {
        var records = await GetByReptileIdAsync(reptileId);
        return records.Select(CreateDtoFromRecord).ToList();
    }

    public async Task<FeedingRecord?> GetByIdAsync(int id)
    {
        return await _db.FeedingRecords
            .Include(f => f.Reptile)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<FeedingRecordDto?> GetDtoByIdAsync(int id)
    {
        var record = await GetByIdAsync(id);
        return record != null ? CreateDtoFromRecord(record) : null;
    }

    public async Task<FeedingRecord> CreateAsync(FeedingRecord feedingRecord)
    {
        await _db.FeedingRecords.AddAsync(feedingRecord);
        await _db.SaveChangesAsync();
        return feedingRecord;
    }

    public async Task<bool> UpdateAsync(FeedingRecord feedingRecord)
    {
        var existingRecord = await _db.FeedingRecords.FindAsync(feedingRecord.Id);
        if (existingRecord == null)
            return false;

        _db.Entry(existingRecord).CurrentValues.SetValues(feedingRecord);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var record = await _db.FeedingRecords.FindAsync(id);
        if (record == null)
            return false;

        _db.FeedingRecords.Remove(record);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<DateTime?> GetLastFeedingDateAsync(int reptileId)
    {
        var lastFeeding = await _db.FeedingRecords
            .Where(f => f.ReptileId == reptileId && f.WasEaten)
            .OrderByDescending(f => f.FeedingDate)
            .FirstOrDefaultAsync();

        return lastFeeding?.FeedingDate;
    }

    private FeedingRecordDto CreateDtoFromRecord(FeedingRecord record)
    {
        return new FeedingRecordDto
        {
            Id = record.Id,
            ReptileId = record.ReptileId,
            ReptileName = record.Reptile?.Name ?? string.Empty,
            FeedingDate = record.FeedingDate,
            FoodItem = record.FoodItem,
            Quantity = record.Quantity,
            ItemType = record.ItemType,
            WasEaten = record.WasEaten,
            Notes = record.Notes
        };
    }
}