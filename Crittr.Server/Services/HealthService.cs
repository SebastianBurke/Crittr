using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services;

public class HealthService : IHealthService
{
    private readonly ApplicationDbContext _db;

    public HealthService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<HealthScore>> GetAllAsync()
    {
        return await _db.HealthScores
            .Include(h => h.Critter)
            .OrderByDescending(h => h.AssessmentDate)
            .ToListAsync();
    }

    public async Task<List<HealthScore>> GetByCritterIdAsync(int critterId)
    {
        return await _db.HealthScores
            .Include(h => h.Critter)
            .Where(h => h.CritterId == critterId)
            .OrderBy(h => h.AssessmentDate)
            .ToListAsync();
    }

    public async Task<List<HealthScoreDto>> GetDtosByCritterIdAsync(int critterId)
    {
        var records = await GetByCritterIdAsync(critterId);
        return records.Select(CreateDtoFromRecord).ToList();
    }

    public async Task<HealthScore?> GetByIdAsync(int id)
    {
        return await _db.HealthScores
            .Include(h => h.Critter)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<HealthScoreDto?> GetDtoByIdAsync(int id)
    {
        var record = await GetByIdAsync(id);
        return record != null ? CreateDtoFromRecord(record) : null;
    }

    public async Task<HealthScore> CreateAsync(HealthScore healthScore)
    {
        await _db.HealthScores.AddAsync(healthScore);
        await _db.SaveChangesAsync();
        return healthScore;
    }

    public async Task<bool> UpdateAsync(HealthScore healthScore)
    {
        var existing = await _db.HealthScores.FindAsync(healthScore.Id);
        if (existing == null) return false;

        _db.Entry(existing).CurrentValues.SetValues(healthScore);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var record = await _db.HealthScores.FindAsync(id);
        if (record == null) return false;

        _db.HealthScores.Remove(record);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<int?> GetLatestHealthScoreAsync(int critterId)
    {
        var latest = await _db.HealthScores
            .Where(h => h.CritterId == critterId)
            .OrderByDescending(h => h.AssessmentDate)
            .FirstOrDefaultAsync();

        return latest?.Score;
    }

    public async Task<List<HealthScoreDto>> GetHealthTrendDataAsync(int critterId, int months)
    {
        var cutoff = DateTime.UtcNow.AddMonths(-months);
        var records = await _db.HealthScores
            .Include(h => h.Critter)
            .Where(h => h.CritterId == critterId && h.AssessmentDate >= cutoff)
            .OrderBy(h => h.AssessmentDate)
            .ToListAsync();

        return records.Select(CreateDtoFromRecord).ToList();
    }

    private HealthScoreDto CreateDtoFromRecord(HealthScore record)
    {
        return new HealthScoreDto
        {
            Id = record.Id,
            CritterId = record.CritterId,
            CritterName = record.Critter?.Name ?? string.Empty,
            AssessmentDate = record.AssessmentDate,
            Score = record.Score,
            Notes = record.Notes,
            AssessedBy = record.AssessedBy
        };
    }
}
