using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services;

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
            .Include(e => e.Critter)
            .OrderByDescending(e => e.ReadingDate)
            .ToListAsync();
    }

    public async Task<List<EnvironmentalReading>> GetByCritterIdAsync(int critterId)
    {
        return await _db.EnvironmentalReadings
            .Include(e => e.Critter)
            .Where(e => e.CritterId == critterId)
            .OrderByDescending(e => e.ReadingDate)
            .ToListAsync();
    }

    public async Task<List<EnvironmentalReadingDto>> GetDtosByCritterIdAsync(int critterId)
    {
        var readings = await GetByCritterIdAsync(critterId);
        return readings.Select(CreateDtoFromReading).ToList();
    }

    public async Task<EnvironmentalReading?> GetByIdAsync(int id)
    {
        return await _db.EnvironmentalReadings
            .Include(e => e.Critter)
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
            .Include(e => e.Critter)
            .OrderByDescending(e => e.ReadingDate)
            .Take(count)
            .ToListAsync();

        return readings.Select(CreateDtoFromReading).ToList();
    }

    public async Task<EnvironmentalReadingDto?> GetLatestForCritterAsync(int critterId)
    {
        var reading = await _db.EnvironmentalReadings
            .Include(e => e.Critter)
            .Where(e => e.CritterId == critterId)
            .OrderByDescending(e => e.ReadingDate)
            .FirstOrDefaultAsync();

        return reading != null ? CreateDtoFromReading(reading) : null;
    }

    private EnvironmentalReadingDto CreateDtoFromReading(EnvironmentalReading reading)
    {
        return new EnvironmentalReadingDto
        {
            Id = reading.Id,
            CritterId = reading.CritterId,
            CritterName = reading.Critter?.Name ?? string.Empty,
            ReadingDate = reading.ReadingDate,
            Temperature = reading.Temperature,
            Humidity = reading.Humidity,
            UVBIndex = reading.UVBIndex,
            IsManualReading = reading.IsManualReading,
            Source = reading.Source
        };
    }
}