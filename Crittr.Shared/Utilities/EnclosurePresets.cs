using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Utilities;

public record SizePreset(string Label, double Length, double Width, double Height);

public static class EnclosurePresets
{
    private static readonly SizePreset Custom = new("Custom", 0, 0, 0);

    public static IReadOnlyList<SizePreset> GetSizeOptions(EnclosureType type) => type switch
    {
        EnclosureType.Terrarium => new SizePreset[]
        {
            new("20-gal Long",      30, 12, 12),
            new("40-gal Breeder",   36, 18, 18),
            new("75-gal",           48, 18, 21),
            new("4×2×2 ft",         48, 24, 24),
            new("4×4×2 ft",         48, 48, 24),
            Custom,
        },
        EnclosureType.Aquarium or EnclosureType.Tank => new SizePreset[]
        {
            new("10-gal",           20, 10, 12),
            new("20-gal",           24, 12, 16),
            new("40-gal Breeder",   36, 18, 16),
            new("55-gal",           48, 13, 21),
            new("75-gal",           48, 18, 21),
            Custom,
        },
        EnclosureType.Vivarium => new SizePreset[]
        {
            new("18×18×24″",        18, 18, 24),
            new("18×18×36″",        18, 18, 36),
            new("24×24×48″",        24, 24, 48),
            Custom,
        },
        EnclosureType.Paludarium => new SizePreset[]
        {
            new("20-gal",           24, 12, 16),
            new("40-gal",           36, 18, 16),
            new("75-gal",           48, 18, 21),
            Custom,
        },
        EnclosureType.RackSystem => new SizePreset[]
        {
            new("6-qt tub",         16, 11,  6),
            new("15-qt tub",        21, 15,  7),
            new("32-qt tub",        24, 17, 12),
            new("41-qt tub",        28, 18, 14),
            Custom,
        },
        EnclosureType.Bin => new SizePreset[]
        {
            new("32-qt",            24, 17, 12),
            new("41-qt",            28, 18, 14),
            new("64-qt",            35, 21, 15),
            Custom,
        },
        EnclosureType.Insectarium => new SizePreset[]
        {
            new("Small 8×8×12″",     8,  8, 12),
            new("Medium 12×12×18″", 12, 12, 18),
            new("Large 18×18×24″",  18, 18, 24),
            Custom,
        },
        EnclosureType.Cage => new SizePreset[]
        {
            new("Small 24×18×24″",  24, 18, 24),
            new("Medium 36×24×36″", 36, 24, 36),
            new("Large 48×24×48″",  48, 24, 48),
            Custom,
        },
        EnclosureType.Aviary => new SizePreset[]
        {
            new("Small 36×24×48″",  36, 24, 48),
            new("Large 72×36×72″",  72, 36, 72),
            Custom,
        },
        EnclosureType.FreeRoamRoom => new SizePreset[]
        {
            new("Full Room", 0, 0, 0),
            Custom,
        },
        _ => new SizePreset[] { Custom },
    };

    public static IReadOnlyList<string> GetSubstrateOptions(EnclosureType type) => type switch
    {
        EnclosureType.Terrarium or EnclosureType.Vivarium => new[]
        {
            "Coconut Fibre", "Cypress Mulch", "Bioactive Mix",
            "Topsoil Blend", "Paper Towel", "Reptile Carpet", "Sand", "Custom",
        },
        EnclosureType.Aquarium or EnclosureType.Tank or EnclosureType.Paludarium => new[]
        {
            "Aquarium Gravel", "Fine Sand", "Bare Bottom", "Custom",
        },
        EnclosureType.RackSystem or EnclosureType.Bin => new[]
        {
            "Paper Towel", "Aspen Shavings", "Cypress Mulch", "Custom",
        },
        EnclosureType.Insectarium => new[]
        {
            "Coconut Fibre", "Peat Moss", "Paper Towel", "Custom",
        },
        EnclosureType.Cage => new[]
        {
            "Aspen Shavings", "Paper Towel", "Custom",
        },
        EnclosureType.Aviary => new[]
        {
            "Sand", "Natural Substrate", "Custom",
        },
        EnclosureType.FreeRoamRoom => Array.Empty<string>(),
        _ => new[] { "Custom" },
    };
}
