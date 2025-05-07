using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services;

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
            .Include(f => f.Critter)
            .OrderByDescending(f => f.FeedingDate)
            .ToListAsync();
    }

    public async Task<List<FeedingRecord>> GetByCritterIdAsync(int critterId)
    {
        return await _db.FeedingRecords
            .Include(f => f.Critter)
            .Where(f => f.CritterId == critterId)
            .OrderByDescending(f => f.FeedingDate)
            .ToListAsync();
    }

    public async Task<List<FeedingRecordDto>> GetDtosByCritterIdAsync(int critterId)
    {
        var records = await GetByCritterIdAsync(critterId);
        return records.Select(CreateDtoFromRecord).ToList();
    }

    public async Task<FeedingRecord?> GetByIdAsync(int id)
    {
        return await _db.FeedingRecords
            .Include(f => f.Critter)
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

    public async Task<DateTime?> GetLastFeedingDateAsync(int critterId)
    {
        var lastFeeding = await _db.FeedingRecords
            .Where(f => f.CritterId == critterId && f.WasEaten)
            .OrderByDescending(f => f.FeedingDate)
            .FirstOrDefaultAsync();

        return lastFeeding?.FeedingDate;
    }

    private FeedingRecordDto CreateDtoFromRecord(FeedingRecord record)
    {
        return new FeedingRecordDto
        {
            Id = record.Id,
            CritterId = record.CritterId,
            CritterName = record.Critter?.Name ?? string.Empty,
            FeedingDate = record.FeedingDate,
            FoodItem = record.FoodItem,
            Quantity = record.Quantity,
            ItemType = record.ItemType,
            WasEaten = record.WasEaten,
            Notes = record.Notes
        };
    }
}