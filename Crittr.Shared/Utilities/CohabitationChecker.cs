using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Utilities;

public static class CohabitationChecker
{
    public static CohabitationCheckResultDto Check(
        SpeciesInfo incoming, string? incomingSex,
        IEnumerable<(string PetName, SpeciesInfo Info, string? Sex)> residents)
    {
        var conflicts = new List<CohabitationConflictDto>();

        foreach (var (petName, resident, residentSex) in residents)
        {
            var conflict = CheckPair(incoming, incomingSex, petName, resident, residentSex);
            if (conflict is not null)
                conflicts.Add(conflict);
        }

        return new CohabitationCheckResultDto
        {
            CanCohabit = conflicts.All(c => !c.IsHardBlock),
            HasWarnings = conflicts.Count > 0,
            Conflicts = conflicts,
        };
    }

    private static CohabitationConflictDto? CheckPair(
        SpeciesInfo incoming, string? incomingSex,
        string residentPetName, SpeciesInfo resident, string? residentSex)
    {
        var ip = incoming.CohabProfile;
        var rp = resident.CohabProfile;

        if (ip is null)
            return Warn(residentPetName, resident.CommonName,
                $"Cohabitation profile for {incoming.CommonName} is missing — verify compatibility manually.");
        if (rp is null)
            return Warn(residentPetName, resident.CommonName,
                $"Cohabitation profile for {resident.CommonName} is missing — verify compatibility manually.");

        // Hard block: incoming must live alone
        if (ip.SocialNeeds == SocialStructure.SoloOnly)
            return Block(residentPetName, resident.CommonName,
                WithNote($"{incoming.CommonName} must live alone.", ip.CohabNote));

        // Hard block: resident must live alone
        if (rp.SocialNeeds == SocialStructure.SoloOnly)
            return Block(residentPetName, resident.CommonName,
                WithNote($"{residentPetName} ({resident.CommonName}) must live alone.", rp.CohabNote));

        // Hard block: explicit incompatibility
        if (ip.IncompatibleWith?.Contains(resident.CommonName, StringComparer.OrdinalIgnoreCase) == true ||
            rp.IncompatibleWith?.Contains(incoming.CommonName, StringComparer.OrdinalIgnoreCase) == true)
            return Block(residentPetName, resident.CommonName,
                $"{incoming.CommonName} and {residentPetName} ({resident.CommonName}) are explicitly incompatible.");

        // Warning: incoming is predatory
        if (ip.IsPredatory)
            return Warn(residentPetName, resident.CommonName,
                WithNote($"{incoming.CommonName} is predatory and could harm {residentPetName} ({resident.CommonName}).", ip.CohabNote));

        // Warning: resident is predatory
        if (rp.IsPredatory)
            return Warn(residentPetName, resident.CommonName,
                WithNote($"{residentPetName} ({resident.CommonName}) is predatory and could harm {incoming.CommonName}.", rp.CohabNote));

        // Gender-based conflict — same species only
        if (incoming.CommonName.Equals(resident.CommonName, StringComparison.OrdinalIgnoreCase)
            && ip.GenderRule == GenderPairingRule.OneMalePerEnclosure)
        {
            var inIsMale   = IsMale(incomingSex);
            var resIsMale  = IsMale(residentSex);
            var inUnknown  = IsUnknownSex(incomingSex);
            var resUnknown = IsUnknownSex(residentSex);

            if (inIsMale && resIsMale)
            {
                var reason = ip.MaleMaleIsLethal
                    ? WithNote($"Two male {incoming.CommonName}s will fight — this can be fatal.", ip.CohabNote)
                    : WithNote($"Two male {incoming.CommonName}s may fight and injure each other.", ip.CohabNote);
                return ip.MaleMaleIsLethal
                    ? Block(residentPetName, resident.CommonName, reason)
                    : Warn(residentPetName, resident.CommonName, reason);
            }

            if ((inIsMale && resUnknown) || (inUnknown && resIsMale))
                return Warn(residentPetName, resident.CommonName,
                    WithNote($"Sex of one critter is unknown — if both are male, two male {incoming.CommonName}s may fight.", ip.CohabNote));
        }

        // Warning: different animal types and neither is a community species
        if (incoming.Type != resident.Type &&
            ip.SocialNeeds != SocialStructure.Community &&
            rp.SocialNeeds != SocialStructure.Community)
            return Warn(residentPetName, resident.CommonName,
                $"{incoming.CommonName} ({incoming.Type}) and {residentPetName} ({resident.CommonName}, {resident.Type}) are different animal types and may not be compatible.");

        return null;
    }

    private static bool IsMale(string? sex)
        => string.Equals(sex?.Trim(), "Male", StringComparison.OrdinalIgnoreCase);

    private static bool IsUnknownSex(string? sex)
        => string.IsNullOrWhiteSpace(sex) ||
           string.Equals(sex.Trim(), "Unknown", StringComparison.OrdinalIgnoreCase);

    private static string WithNote(string reason, string? note)
        => string.IsNullOrWhiteSpace(note) ? reason : $"{reason} {note}.";

    private static CohabitationConflictDto Block(string petName, string speciesName, string reason)
        => new() { CritterName = petName, SpeciesName = speciesName, Reason = reason, IsHardBlock = true };

    private static CohabitationConflictDto Warn(string petName, string speciesName, string reason)
        => new() { CritterName = petName, SpeciesName = speciesName, Reason = reason, IsHardBlock = false };
}
