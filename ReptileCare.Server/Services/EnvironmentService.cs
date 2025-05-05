using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.Data;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

namespace ReptileCare.Server.Services;

// ReptileCare.Server/Services/EnvironmentService.cs
using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.Data;
using ReptileCare.Shared.DTOs;
using ReptileCare.Shared.Models;

public class EnvironmentService : IEnvironmentService
{
    private readonly ApplicationDbContext _db;

    public EnvironmentService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<EnvironmentalReading>> GetAllAsync()
    {
        return await _db.EnvironmentalReadings
            .Include(e => e.Reptile)
            .OrderByDescending(e => e.ReadingDate)
            .ToListAsync();
    }

    public async Task<List<EnvironmentalReading>> GetByReptileIdAsync(int reptileId)
    {
        return await _db.EnvironmentalReadings
            .Include(e => e.Reptile)
            .Where(e => e.ReptileId == reptileId)
            .OrderByDescending(e => e.ReadingDate)
            .ToListAsync();
    }

    public async Task<List<EnvironmentalReadingDto>> GetDtosByReptileIdAsync(int reptileId)
    {
        var readings = await GetByReptileIdAsync(reptileId);
        return readings.Select(CreateDtoFromReading).ToList();
    }

    public async Task<EnvironmentalReading?> GetByIdAsync(int id)
    {
        return await _db.EnvironmentalReadings
            .Include(e => e.Reptile)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EnvironmentalReadingDto?> GetDtoByIdAsync(int id)
    {
        var reading = await GetByIdAsync(id);
        return reading != null ? CreateDtoFromReading(reading) : null;
    }

    public async Task<EnvironmentalReading> CreateAsync(EnvironmentalReading reading)
    {
        await _db.EnvironmentalReadings.AddAsync(reading);
        await _db.SaveChangesAsync();
        return reading;
    }

    public async Task<bool> UpdateAsync(EnvironmentalReading reading)
    {
        var existingReading = await _db.EnvironmentalReadings.FindAsync(reading.Id);
        if (existingReading == null)
            return false;

        _db.Entry(existingReading).CurrentValues.SetValues(reading);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var reading = await _db.EnvironmentalReadings.FindAsync(id);
        if (reading == null)
            return false;

        _db.EnvironmentalReadings.Remove(reading);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<EnvironmentalReadingDto>> GetLatestReadingsAsync(int count)
    {
        var readings = await _db.EnvironmentalReadings
            .Include(e => e.Reptile)
            .OrderByDescending(e => e.ReadingDate)
            .Take(count)
            .ToListAsync();

        return readings.Select(CreateDtoFromReading).ToList();
    }

    public async Task<EnvironmentalReadingDto?> GetLatestForReptileAsync(int reptileId)
    {
        var reading = await _db.EnvironmentalReadings
            .Include(e => e.Reptile)
            .Where(e => e.ReptileId == reptileId)
            .OrderByDescending(e => e.ReadingDate)
            .FirstOrDefaultAsync();

        return reading != null ? CreateDtoFromReading(reading) : null;
    }

    private EnvironmentalReadingDto CreateDtoFromReading(EnvironmentalReading reading)
    {
        return new EnvironmentalReadingDto
        {
            Id = reading.Id,
            ReptileId = reading.ReptileId,
            ReptileName = reading.Reptile?.Name ?? string.Empty,
            ReadingDate = reading.ReadingDate,
            Temperature = reading.Temperature,
            Humidity = reading.Humidity,
            UVBIndex = reading.UVBIndex,
            IsManualReading = reading.IsManualReading,
            Source = reading.Source
        };
    }
}