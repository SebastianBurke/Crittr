using Crittr.Server.Data;
using Crittr.Shared.DTOs;
using Crittr.Shared.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Crittr.Server.Services;

public class EnclosureCohabitationService
{
    private readonly ApplicationDbContext _db;
    private readonly SpeciesCatalogService _catalog;

    public EnclosureCohabitationService(ApplicationDbContext db, SpeciesCatalogService catalog)
    {
        _db = db;
        _catalog = catalog;
    }

    public async Task<CohabitationCheckResultDto> CheckAsync(
        int incomingCritterId, int enclosureId, string userId)
    {
        var incoming = await _db.Critters
            .FirstOrDefaultAsync(c => c.Id == incomingCritterId && c.UserId == userId);

        if (incoming is null)
            return Compatible();

        var residents = await _db.Critters
            .Where(c => c.EnclosureProfileId == enclosureId &&
                        c.UserId == userId &&
                        c.Id != incomingCritterId)
            .ToListAsync();

        if (residents.Count == 0)
            return Compatible();

        var incomingInfo = await _catalog.GetByNameAsync(incoming.Species);
        if (incomingInfo is null)
            return UnknownSpecies(incoming.Name, incoming.Species);

        var residentPairs = new List<(string PetName, SpeciesInfo Info, string? Sex)>();
        var unknownResidents = new List<string>();
        foreach (var r in residents)
        {
            var info = await _catalog.GetByNameAsync(r.Species);
            if (info is not null)
                residentPairs.Add((r.Name, info, r.Sex));
            else
                unknownResidents.Add($"{r.Name} ({r.Species})");
        }

        // Run the check against known residents; collect any unknown-resident warnings
        var result = residentPairs.Count > 0
            ? CohabitationChecker.Check(incomingInfo, incoming.Sex, residentPairs)
            : Compatible();

        // Append warnings for any residents whose species isn't in the catalog
        foreach (var label in unknownResidents)
        {
            result.HasWarnings = true;
            result.Conflicts.Add(new CohabitationConflictDto
            {
                CritterName = label,
                SpeciesName = string.Empty,
                Reason = $"{label} isn't in the species catalog — verify compatibility manually.",
                IsHardBlock = false,
            });
        }

        return result;
    }

    private static CohabitationCheckResultDto Compatible()
        => new() { CanCohabit = true, HasWarnings = false, Conflicts = [] };

    private static CohabitationCheckResultDto UnknownSpecies(string petName, string speciesName)
        => new()
        {
            CanCohabit = true,
            HasWarnings = true,
            Conflicts =
            [
                new CohabitationConflictDto
                {
                    CritterName = petName,
                    SpeciesName = speciesName,
                    Reason = $"{speciesName} isn't in the species catalog — cohabitation rules can't be verified.",
                    IsHardBlock = false,
                }
            ]
        };
}
