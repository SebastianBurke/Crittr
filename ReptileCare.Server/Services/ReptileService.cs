using ReptileCare.Shared.Models;
using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Data;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.DTOs;
using IFeedingService = ReptileCare.Server.Services.Interfaces.IFeedingService;
using IReptileService = ReptileCare.Server.Services.Interfaces.IReptileService;

namespace ReptileCare.Server.Services;

public class ReptileService : IReptileService
{
    private readonly ApplicationDbContext _db;
    private readonly IFeedingService _feedingService;
    private readonly ISheddingService _sheddingService;
    private readonly IMeasurementService _measurementService;
    private readonly IHealthService _healthService;
    private readonly IScheduledTaskService _scheduledTaskService;

    public ReptileService(
        ApplicationDbContext db,
        IFeedingService feedingService,
        ISheddingService sheddingService,
        IMeasurementService measurementService,
        IHealthService healthService,
        IScheduledTaskService scheduledTaskService)
    {
        _db = db;
        _feedingService = feedingService;
        _sheddingService = sheddingService;
        _measurementService = measurementService;
        _healthService = healthService;
        _scheduledTaskService = scheduledTaskService;
    }

    public async Task<List<Reptile>> GetAllAsync()
    {
        return await _db.Reptiles
            .Include(r => r.EnclosureProfile)
            .ToListAsync();
    }

    public async Task<Reptile?> GetByIdAsync(int id)
    {
        return await _db.Reptiles
            .Include(r => r.EnclosureProfile)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<ReptileDto?> GetDtoByIdAsync(int id)
    {
        var reptile = await GetByIdAsync(id);
        if (reptile == null)
            return null;

        return await CreateDtoFromReptile(reptile);
    }

    public async Task<List<ReptileDto>> GetAllDtosByUserIdAsync(string userId)
    {
        return await _db.Reptiles
            .Where(r => r.OwnerId == userId)
            .Select(r => new ReptileDto
            {
                Id = r.Id,
                Name = r.Name,
                Species = r.Species,
                DateAcquired = r.DateAcquired,
                EnclosureProfileId = r.EnclosureProfileId,
                RecentHealthScore = r.HealthScores
                    .OrderByDescending(h => h.AssessmentDate).Select(h => h.Score).FirstOrDefault(),
                LastFeedingDate = r.FeedingRecords
                    .OrderByDescending(f => f.FeedingDate).Select(f => f.FeedingDate).FirstOrDefault(),
                LastWeightDate = r.MeasurementRecords
                    .OrderByDescending(m => m.MeasurementDate).Select(m => m.MeasurementDate).FirstOrDefault(),
                PendingTasksCount = r.ScheduledTasks.Count(t => !t.IsCompleted)
            })
            .ToListAsync();
    }


    public async Task<List<ReptileDto>> GetAllDtosAsync()
    {
        var reptiles = await GetAllAsync();
        var dtos = new List<ReptileDto>();

        foreach (var reptile in reptiles)
        {
            dtos.Add(await CreateDtoFromReptile(reptile));
        }

        return dtos;
    }

    public async Task<Reptile> CreateAsync(Reptile reptile)
    {
        await _db.Reptiles.AddAsync(reptile);
        await _db.SaveChangesAsync();
        return reptile;
    }

    public async Task<bool> UpdateAsync(Reptile reptile)
    {
        var existingReptile = await _db.Reptiles.FindAsync(reptile.Id);
        if (existingReptile == null)
            return false;

        _db.Entry(existingReptile).CurrentValues.SetValues(reptile);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var reptile = await _db.Reptiles.FindAsync(id);
        if (reptile == null)
            return false;

        _db.Reptiles.Remove(reptile);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ReptileDto>> SearchAsync(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        var reptiles = await _db.Reptiles
            .Include(r => r.EnclosureProfile)
            .Where(r => r.Name.ToLower().Contains(searchTerm) ||
                        r.Species.ToLower().Contains(searchTerm) ||
                        r.Description != null && r.Description.ToLower().Contains(searchTerm))
            .ToListAsync();

        var dtos = new List<ReptileDto>();
        foreach (var reptile in reptiles)
        {
            dtos.Add(await CreateDtoFromReptile(reptile));
        }

        return dtos;
    }

    private async Task<ReptileDto> CreateDtoFromReptile(Reptile reptile)
    {
        var dto = new ReptileDto
        {
            Id = reptile.Id,
            Name = reptile.Name,
            Species = reptile.Species,
            DateAcquired = reptile.DateAcquired,
            DateOfBirth = reptile.DateOfBirth,
            Sex = reptile.Sex,
            Weight = reptile.Weight,
            Length = reptile.Length,
            Description = reptile.Description,
            EnclosureProfileId = reptile.EnclosureProfileId,
            LastFeedingDate = await _feedingService.GetLastFeedingDateAsync(reptile.Id),
            LastSheddingDate = await _sheddingService.GetLastSheddingDateAsync(reptile.Id),
            RecentHealthScore = await _healthService.GetLatestHealthScoreAsync(reptile.Id),
            PendingTasksCount = await _scheduledTaskService.GetPendingTasksCountAsync(reptile.Id)
        };

        // Get the last measurement record for more accurate weight
        var lastMeasurement = await _measurementService.GetLatestForReptileAsync(reptile.Id);
        if (lastMeasurement != null)
        {
            dto.LastWeightDate = lastMeasurement.MeasurementDate;
            dto.Weight = lastMeasurement.Weight;
            dto.Length = lastMeasurement.Length;
        }

        return dto;
    }
}