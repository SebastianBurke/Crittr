using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;

namespace Crittr.Server.Services;

public class EnclosureService : IEnclosureService
{
    private readonly ApplicationDbContext _db;

    public EnclosureService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<EnclosureProfile>> GetAllAsync()
    {
        return await _db.EnclosureProfiles.ToListAsync();
    }

    public async Task<EnclosureProfile?> GetByIdAsync(int id)
    {
        return await _db.EnclosureProfiles.FindAsync(id);
    }

    public async Task<EnclosureProfileDto?> GetDtoByIdAsync(int id)
    {
        var enclosure = await GetByIdAsync(id);
        return enclosure == null ? null : MapToDto(enclosure);
    }

    public async Task<List<EnclosureProfileDto>> GetAllDtosByUserIdAsync(string userId)
    {
        return await _db.EnclosureProfiles
            .Where(e => e.OwnerId == userId)
            .Select(e => new EnclosureProfileDto
            {
                Id = e.Id,
                Name = e.Name,
                SubstrateType = e.SubstrateType,
                Length = e.Length,
                Width = e.Width,
                Height = e.Height,
                HasUVBLighting = e.HasUVBLighting,
                HasHeatingElement = e.HasHeatingElement,
                IdealTemperature = e.IdealTemperature,
                IdealHumidity = e.IdealHumidity
            }).ToListAsync();
    }

    public async Task<EnclosureProfile> CreateAsync(EnclosureProfile enclosure)
    {
        _db.EnclosureProfiles.Add(enclosure);
        await _db.SaveChangesAsync();
        return enclosure;
    }

    public async Task<bool> UpdateAsync(EnclosureProfile enclosure)
    {
        var existing = await _db.EnclosureProfiles.FindAsync(enclosure.Id);
        if (existing == null) return false;

        _db.Entry(existing).CurrentValues.SetValues(enclosure);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var enclosure = await _db.EnclosureProfiles.FindAsync(id);
        if (enclosure == null) return false;

        _db.EnclosureProfiles.Remove(enclosure);
        await _db.SaveChangesAsync();
        return true;
    }

    public static EnclosureProfileDto MapToDto(EnclosureProfile e)
    {
        return new EnclosureProfileDto
        {
            Id = e.Id,
            Name = e.Name,
            SubstrateType = e.SubstrateType,
            Length = e.Length,
            Width = e.Width,
            Height = e.Height,
            HasUVBLighting = e.HasUVBLighting,
            HasHeatingElement = e.HasHeatingElement,
            IdealTemperature = e.IdealTemperature,
            IdealHumidity = e.IdealHumidity
        };
    }
    
    public async Task<List<EnclosureProfileDto>> GetAllDtosAsync()
    {
        return await _db.EnclosureProfiles
            .Select(e => new EnclosureProfileDto
            {
                Id = e.Id,
                Name = e.Name,
                SubstrateType = e.SubstrateType,
                Length = e.Length,
                Width = e.Width,
                Height = e.Height,
                HasUVBLighting = e.HasUVBLighting,
                HasHeatingElement = e.HasHeatingElement,
                IdealTemperature = e.IdealTemperature,
                IdealHumidity = e.IdealHumidity
            }).ToListAsync();
    }

    public async Task<bool> AssignToCritterAsync(int critterId, int enclosureId)
    {
        var critter = await _db.Critters.FindAsync(critterId);
        var enclosure = await _db.EnclosureProfiles.FindAsync(enclosureId);

        if (critter == null || enclosure == null)
            return false;

        critter.EnclosureProfileId = enclosureId;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnassignFromCritterAsync(int critterId)
    {
        var critter = await _db.Critters.FindAsync(critterId);
        if (critter == null)
            return false;

        critter.EnclosureProfileId = null;
        await _db.SaveChangesAsync();
        return true;
    }
    
}
