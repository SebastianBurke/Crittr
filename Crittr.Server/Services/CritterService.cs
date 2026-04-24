using Crittr.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;
using IFeedingService = Crittr.Server.Services.Interfaces.IFeedingService;
using ICritterService = Crittr.Server.Services.Interfaces.ICritterService;

namespace Crittr.Server.Services;

public class CritterService : ICritterService
{
    private readonly ApplicationDbContext _db;
    private readonly IFeedingService _feedingService;
    private readonly ISheddingService _sheddingService;
    private readonly IMeasurementService _measurementService;
    private readonly IHealthService _healthService;
    private readonly IScheduledTaskService _scheduledTaskService;
    private readonly IHealthAnalyticsEngine _analyticsEngine;
    private readonly SpeciesCatalogService _catalog;

    public CritterService(
        ApplicationDbContext db,
        IFeedingService feedingService,
        ISheddingService sheddingService,
        IMeasurementService measurementService,
        IHealthService healthService,
        IScheduledTaskService scheduledTaskService,
        IHealthAnalyticsEngine analyticsEngine,
        SpeciesCatalogService catalog)
    {
        _db = db;
        _feedingService = feedingService;
        _sheddingService = sheddingService;
        _measurementService = measurementService;
        _healthService = healthService;
        _scheduledTaskService = scheduledTaskService;
        _analyticsEngine = analyticsEngine;
        _catalog = catalog;
    }

    public async Task<List<Critter>> GetAllAsync()
    {
        return await _db.Critters
            .Include(r => r.EnclosureProfile)
            .ToListAsync();
    }

    public async Task<Critter?> GetByIdAsync(int id)
    {
        return await _db.Critters
            .Include(r => r.EnclosureProfile)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<CritterDto?> GetDtoByIdAsync(int id)
    {
        var critter = await GetByIdAsync(id);
        if (critter == null)
            return null;

        return await CreateDtoFromCritter(critter);
    }

    public async Task<List<CritterDto>> GetAllDtosByEnclosureIdAsync(int enclosureId)
    {
        var dtos = await _db.Critters
            .Where(r => r.EnclosureProfileId == enclosureId)
            .Select(r => new CritterDto
            {
                Id = r.Id,
                Name = r.Name,
                Species = r.Species,
                SpeciesType = r.SpeciesType,
                DateAcquired = r.DateAcquired,
                DateOfBirth = r.DateOfBirth,
                Sex = r.Sex,
                Weight = r.Weight,
                Length = r.Length,
                Description = r.Description,
                EnclosureProfileId = r.EnclosureProfileId,
                UserId = r.UserId,
                IconUrl = r.IconUrl,
                RecentHealthScore = r.HealthScores
                    .OrderByDescending(h => h.AssessmentDate)
                    .Select(h => (int?)h.Score)
                    .FirstOrDefault(),
                LastFeedingDate = r.FeedingRecords
                    .Where(f => f.WasEaten)
                    .OrderByDescending(f => f.FeedingDate)
                    .Select(f => (DateTime?)f.FeedingDate)
                    .FirstOrDefault(),
                LastWeightDate = r.MeasurementRecords
                    .OrderByDescending(m => m.MeasurementDate)
                    .Select(m => (DateTime?)m.MeasurementDate)
                    .FirstOrDefault(),
                LastSheddingDate = r.SheddingRecords
                    .OrderByDescending(s => s.CompletionDate)
                    .Select(s => (DateTime?)s.CompletionDate)
                    .FirstOrDefault(),
                PendingTasksCount = r.ScheduledTasks.Count(t => !t.IsCompleted)
            })
            .ToListAsync();
        ComputeConditions(dtos);
        return dtos;
    }

    public async Task<List<CritterDto>> GetAllDtosAsync()
    {
        var dtos = await _db.Critters
            .Select(r => new CritterDto
            {
                Id = r.Id,
                Name = r.Name,
                Species = r.Species,
                SpeciesType = r.SpeciesType,
                DateAcquired = r.DateAcquired,
                DateOfBirth = r.DateOfBirth,
                Sex = r.Sex,
                Weight = r.Weight,
                Length = r.Length,
                Description = r.Description,
                EnclosureProfileId = r.EnclosureProfileId,
                UserId = r.UserId,
                IconUrl = r.IconUrl,
                RecentHealthScore = r.HealthScores
                    .OrderByDescending(h => h.AssessmentDate)
                    .Select(h => (int?)h.Score)
                    .FirstOrDefault(),
                LastFeedingDate = r.FeedingRecords
                    .Where(f => f.WasEaten)
                    .OrderByDescending(f => f.FeedingDate)
                    .Select(f => (DateTime?)f.FeedingDate)
                    .FirstOrDefault(),
                LastWeightDate = r.MeasurementRecords
                    .OrderByDescending(m => m.MeasurementDate)
                    .Select(m => (DateTime?)m.MeasurementDate)
                    .FirstOrDefault(),
                LastSheddingDate = r.SheddingRecords
                    .OrderByDescending(s => s.CompletionDate)
                    .Select(s => (DateTime?)s.CompletionDate)
                    .FirstOrDefault(),
                PendingTasksCount = r.ScheduledTasks.Count(t => !t.IsCompleted)
            })
            .ToListAsync();
        ComputeConditions(dtos);
        return dtos;
    }

    public async Task<List<CritterDto>> GetUnassignedCrittersByUserAsync(string userId)
    {
        var dtos = await _db.Critters
            .Where(c => c.UserId == userId && c.EnclosureProfileId == null)
            .Select(r => new CritterDto
            {
                Id = r.Id,
                Name = r.Name,
                Species = r.Species,
                SpeciesType = r.SpeciesType,
                DateAcquired = r.DateAcquired,
                DateOfBirth = r.DateOfBirth,
                Sex = r.Sex,
                Weight = r.Weight,
                Length = r.Length,
                Description = r.Description,
                EnclosureProfileId = r.EnclosureProfileId,
                UserId = r.UserId,
                IconUrl = r.IconUrl,
                RecentHealthScore = r.HealthScores
                    .OrderByDescending(h => h.AssessmentDate)
                    .Select(h => (int?)h.Score)
                    .FirstOrDefault(),
                LastFeedingDate = r.FeedingRecords
                    .Where(f => f.WasEaten)
                    .OrderByDescending(f => f.FeedingDate)
                    .Select(f => (DateTime?)f.FeedingDate)
                    .FirstOrDefault(),
                LastWeightDate = r.MeasurementRecords
                    .OrderByDescending(m => m.MeasurementDate)
                    .Select(m => (DateTime?)m.MeasurementDate)
                    .FirstOrDefault(),
                LastSheddingDate = r.SheddingRecords
                    .OrderByDescending(s => s.CompletionDate)
                    .Select(s => (DateTime?)s.CompletionDate)
                    .FirstOrDefault(),
                PendingTasksCount = r.ScheduledTasks.Count(t => !t.IsCompleted)
            })
            .ToListAsync();
        ComputeConditions(dtos);
        return dtos;
    }

    public async Task<List<Critter>> GetAllByUserAsync(string userId)
    {
        return await _db.Critters
            .Include(r => r.EnclosureProfile)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<CritterDto>> SearchByUserAsync(string searchTerm, string userId)
    {
        searchTerm = searchTerm.ToLower();
        var critters = await _db.Critters
            .Include(r => r.EnclosureProfile)
            .Where(r => r.UserId == userId &&
                        (r.Name.ToLower().Contains(searchTerm) ||
                         r.Species.ToLower().Contains(searchTerm) ||
                         r.Description != null && r.Description.ToLower().Contains(searchTerm)))
            .ToListAsync();

        var dtos = new List<CritterDto>();
        foreach (var critter in critters)
            dtos.Add(await CreateDtoFromCritter(critter));
        return dtos;
    }

    public async Task<Critter> CreateAsync(Critter critter)
    {
        await _db.Critters.AddAsync(critter);
        await _db.SaveChangesAsync();
        return critter;
    }

    public async Task<bool> UpdateAsync(Critter critter)
    {
        var existingCritter = await _db.Critters.FindAsync(critter.Id);
        if (existingCritter == null)
            return false;

        _db.Entry(existingCritter).CurrentValues.SetValues(critter);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var critter = await _db.Critters.FindAsync(id);
        if (critter == null)
            return false;

        _db.Critters.Remove(critter);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<CritterDto>> SearchAsync(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        var critters = await _db.Critters
            .Include(r => r.EnclosureProfile)
            .Where(r => r.Name.ToLower().Contains(searchTerm) ||
                        r.Species.ToLower().Contains(searchTerm) ||
                        r.Description != null && r.Description.ToLower().Contains(searchTerm))
            .ToListAsync();

        var dtos = new List<CritterDto>();
        foreach (var critter in critters)
        {
            dtos.Add(await CreateDtoFromCritter(critter));
        }

        return dtos;
    }

    private void ComputeConditions(List<CritterDto> dtos)
    {
        foreach (var dto in dtos)
        {
            if (dto.EnclosureProfileId == null) { dto.Condition = CritterCondition.Unknown; continue; }
            // EF's FirstOrDefault on a non-nullable DateTime column can return DateTime.MinValue when no rows match.
            // Treat any suspiciously early date as "no data".
            if (dto.LastFeedingDate.HasValue && dto.LastFeedingDate.Value.Year < 2000) dto.LastFeedingDate = null;
            if (dto.LastSheddingDate.HasValue && dto.LastSheddingDate.Value.Year < 2000) dto.LastSheddingDate = null;
            dto.Condition = _analyticsEngine.ComputeCondition(dto, _catalog.GetCareProfileByCommonName(dto.Species));
        }
    }

    private async Task<CritterDto> CreateDtoFromCritter(Critter critter)
    {
        var dto = new CritterDto
        {
            Id = critter.Id,
            Name = critter.Name,
            Species = critter.Species,
            SpeciesType = critter.SpeciesType,
            DateAcquired = critter.DateAcquired,
            DateOfBirth = critter.DateOfBirth,
            Sex = critter.Sex,
            Weight = critter.Weight,
            Length = critter.Length,
            Description = critter.Description,
            EnclosureProfileId = critter.EnclosureProfileId,
            IconUrl = critter.IconUrl,
            UserId = critter.UserId,
            LastFeedingDate = await _feedingService.GetLastFeedingDateAsync(critter.Id),
            LastSheddingDate = await _sheddingService.GetLastSheddingDateAsync(critter.Id),
            RecentHealthScore = await _healthService.GetLatestHealthScoreAsync(critter.Id),
            PendingTasksCount = await _scheduledTaskService.GetPendingTasksCountAsync(critter.Id)
        };

        // Get the last measurement record for more accurate weight
        var lastMeasurement = await _measurementService.GetLatestForCritterAsync(critter.Id);
        if (lastMeasurement != null)
        {
            dto.LastWeightDate = lastMeasurement.MeasurementDate;
            dto.Weight = lastMeasurement.Weight;
            dto.Length = lastMeasurement.Length;
        }

        if (critter.EnclosureProfileId == null)
        {
            dto.Condition = CritterCondition.Unknown;
            return dto;
        }

        var careProfile = _catalog.GetCareProfileByCommonName(critter.Species);
        dto.Condition = _analyticsEngine.ComputeCondition(dto, careProfile);

        return dto;
    }
}