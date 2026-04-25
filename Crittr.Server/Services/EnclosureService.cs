using Microsoft.EntityFrameworkCore;
using Crittr.Server.Data;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models;
using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;

namespace Crittr.Server.Services;

public class EnclosureService : IEnclosureService
{
    private readonly ApplicationDbContext _db;

    public EnclosureService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<EnclosureProfile?> GetByIdAsync(int id)
    {
        return await _db.EnclosureProfiles.FindAsync(id);
    }

    public async Task<EnclosureProfile?> GetByIdAsync(int id, string ownerId)
    {
        return await _db.EnclosureProfiles
            .FirstOrDefaultAsync(e => e.Id == id && e.OwnerId == ownerId);
    }

    public async Task<EnclosureProfileDto?> GetDtoByIdAsync(int id)
    {
        var enclosure = await GetByIdAsync(id);
        return enclosure == null ? null : MapToDto(enclosure);
    }

    public async Task<EnclosureProfileDto?> GetDtoByIdAsync(int id, string ownerId)
    {
        var enclosure = await GetByIdAsync(id, ownerId);
        return enclosure == null ? null : MapToDto(enclosure);
    }

    public async Task<List<EnclosureProfileDto>> GetAllDtosByUserIdAsync(string userId)
    {
        return await GetAllDtosByUserIdAsync(userId, 100, 0);
    }

    public async Task<List<EnclosureProfileDto>> GetAllDtosByUserIdAsync(string userId, int take, int skip)
    {
        if (take < 1) take = 1;
        if (take > 200) take = 200;
        if (skip < 0) skip = 0;

        return await _db.EnclosureProfiles
            .Where(e => e.OwnerId == userId)
            .OrderBy(e => e.Id)
            .Skip(skip)
            .Take(take)
            .Select(e => new EnclosureProfileDto
            {
                Id = e.Id,
                Name = e.Name,
                EnclosureType = e.EnclosureType,
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

    public async Task<bool> DeleteAsync(int id, string ownerId)
    {
        var enclosure = await _db.EnclosureProfiles
            .FirstOrDefaultAsync(e => e.Id == id && e.OwnerId == ownerId);
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
            EnclosureType = e.EnclosureType,
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

    public async Task<List<EnclosureProfileDto>> GetCompatibleByUserIdAsync(string userId, SpeciesType speciesType)
    {
        var compatibleTypes = EnclosureCompatibility.GetCompatibleEnclosureTypes(speciesType);
        return await _db.EnclosureProfiles
            .Where(e => e.OwnerId == userId && compatibleTypes.Contains(e.EnclosureType))
            .Select(e => new EnclosureProfileDto
            {
                Id = e.Id,
                Name = e.Name,
                EnclosureType = e.EnclosureType,
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

    public async Task<bool> UpdateFromDtoAsync(EnclosureProfileDto dto, string ownerId)
    {
        var existing = await _db.EnclosureProfiles.FindAsync(dto.Id);
        if (existing == null || existing.OwnerId != ownerId) return false;

        existing.Name = dto.Name;
        existing.EnclosureType = dto.EnclosureType;
        existing.Length = dto.Length;
        existing.Width = dto.Width;
        existing.Height = dto.Height;
        existing.SubstrateType = dto.SubstrateType;
        existing.HasUVBLighting = dto.HasUVBLighting;
        existing.HasHeatingElement = dto.HasHeatingElement;
        existing.IdealTemperature = dto.IdealTemperature;
        existing.IdealHumidity = dto.IdealHumidity;

        await _db.SaveChangesAsync();
        return true;
    }
}
