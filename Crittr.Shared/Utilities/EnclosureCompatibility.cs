using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Utilities;

public static class EnclosureCompatibility
{
    private static readonly Dictionary<SpeciesType, IReadOnlyList<EnclosureType>> SpeciesToEnclosure = new()
    {
        [SpeciesType.Reptile]      = [EnclosureType.Terrarium, EnclosureType.Vivarium, EnclosureType.Tank, EnclosureType.Bin, EnclosureType.RackSystem, EnclosureType.FreeRoamRoom, EnclosureType.Other],
        [SpeciesType.Amphibian]    = [EnclosureType.Terrarium, EnclosureType.Vivarium, EnclosureType.Paludarium, EnclosureType.Aquarium, EnclosureType.Other],
        [SpeciesType.Bird]         = [EnclosureType.Aviary, EnclosureType.Cage, EnclosureType.FreeRoamRoom, EnclosureType.Other],
        [SpeciesType.Fish]         = [EnclosureType.Aquarium, EnclosureType.Paludarium, EnclosureType.Tank, EnclosureType.Other],
        [SpeciesType.Mammal]       = [EnclosureType.Cage, EnclosureType.FreeRoamRoom, EnclosureType.Bin, EnclosureType.Other],
        [SpeciesType.Invertebrate] = [EnclosureType.Terrarium, EnclosureType.Insectarium, EnclosureType.Bin, EnclosureType.Other],
        [SpeciesType.Unknown]      = [.. Enum.GetValues<EnclosureType>()],
    };

    private static readonly Dictionary<EnclosureType, IReadOnlyList<SpeciesType>> EnclosureToSpecies = new()
    {
        [EnclosureType.Terrarium]    = [SpeciesType.Reptile, SpeciesType.Amphibian, SpeciesType.Invertebrate],
        [EnclosureType.Aquarium]     = [SpeciesType.Fish, SpeciesType.Amphibian, SpeciesType.Invertebrate],
        [EnclosureType.Paludarium]   = [SpeciesType.Amphibian, SpeciesType.Reptile, SpeciesType.Fish, SpeciesType.Invertebrate],
        [EnclosureType.Vivarium]     = [SpeciesType.Reptile, SpeciesType.Amphibian, SpeciesType.Invertebrate],
        [EnclosureType.Insectarium]  = [SpeciesType.Invertebrate],
        [EnclosureType.Aviary]       = [SpeciesType.Bird],
        [EnclosureType.Cage]         = [SpeciesType.Bird, SpeciesType.Mammal],
        [EnclosureType.Bin]          = [SpeciesType.Reptile, SpeciesType.Mammal, SpeciesType.Invertebrate],
        [EnclosureType.RackSystem]   = [SpeciesType.Reptile],
        [EnclosureType.FreeRoamRoom] = [SpeciesType.Mammal, SpeciesType.Bird],
        [EnclosureType.Tank]         = [SpeciesType.Reptile, SpeciesType.Fish, SpeciesType.Invertebrate],
        [EnclosureType.Other]        = [.. Enum.GetValues<SpeciesType>()],
    };

    private static readonly Dictionary<SpeciesType, EnclosureType> IdealEnclosure = new()
    {
        [SpeciesType.Reptile]      = EnclosureType.Terrarium,
        [SpeciesType.Amphibian]    = EnclosureType.Vivarium,
        [SpeciesType.Bird]         = EnclosureType.Aviary,
        [SpeciesType.Fish]         = EnclosureType.Aquarium,
        [SpeciesType.Mammal]       = EnclosureType.Cage,
        [SpeciesType.Invertebrate] = EnclosureType.Insectarium,
        [SpeciesType.Unknown]      = EnclosureType.Terrarium,
    };

    private static readonly Dictionary<SpeciesType, string> RequirementLabels = new()
    {
        [SpeciesType.Reptile]      = "a glass terrarium",
        [SpeciesType.Amphibian]    = "a tropical vivarium",
        [SpeciesType.Bird]         = "an aviary or cage",
        [SpeciesType.Fish]         = "an aquarium or tank",
        [SpeciesType.Mammal]       = "a habitat cage",
        [SpeciesType.Invertebrate] = "an insectarium",
        [SpeciesType.Unknown]      = "an enclosure",
    };

    public static bool IsCompatible(SpeciesType species, EnclosureType enclosure)
    {
        if (enclosure == EnclosureType.Other || species == SpeciesType.Unknown) return true;
        return SpeciesToEnclosure.TryGetValue(species, out var types) && types.Contains(enclosure);
    }

    public static IReadOnlyList<EnclosureType> GetCompatibleEnclosureTypes(SpeciesType species)
        => SpeciesToEnclosure.TryGetValue(species, out var types) ? types : [.. Enum.GetValues<EnclosureType>()];

    public static IReadOnlyList<SpeciesType> GetCompatibleSpeciesTypes(EnclosureType enclosure)
        => EnclosureToSpecies.TryGetValue(enclosure, out var types) ? types : [.. Enum.GetValues<SpeciesType>()];

    public static EnclosureType GetIdealEnclosureType(SpeciesType species)
        => IdealEnclosure.TryGetValue(species, out var t) ? t : EnclosureType.Other;

    public static string GetEnclosureRequirementLabel(SpeciesType species)
        => RequirementLabels.TryGetValue(species, out var label) ? label : "an enclosure";

    public static string FormatEnclosureType(EnclosureType t) => t switch
    {
        EnclosureType.RackSystem   => "Rack System",
        EnclosureType.FreeRoamRoom => "Free-Roam Room",
        _                          => t.ToString()
    };
}
