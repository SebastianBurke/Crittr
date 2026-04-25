using System.Reflection;
using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;

namespace Crittr.Tests;

internal static class TestData
{
    public static SpeciesInfo Species(string commonName, SpeciesType type)
    {
        var profile = CohabitationCatalog.Get(commonName)
            ?? throw new InvalidOperationException(
                $"Catalog has no CohabProfile for '{commonName}' — test data drift, update the catalog or the test.");
        return new SpeciesInfo
        {
            CommonName = commonName,
            ScientificName = "—",
            Type = type,
            CohabProfile = profile,
        };
    }

    public static SpeciesInfo Synthetic(string commonName, SpeciesType type, CohabitationProfile? profile)
        => new()
        {
            CommonName = commonName,
            ScientificName = "—",
            Type = type,
            CohabProfile = profile,
        };

    public static (string PetName, SpeciesInfo Info, string? Sex) Resident(SpeciesInfo info, string petName, string? sex = null)
        => (petName, info, sex);

    public static IReadOnlyDictionary<string, CohabitationProfile> AllCatalogEntries()
    {
        var field = typeof(CohabitationCatalog).GetField("_profiles",
            BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("CohabitationCatalog._profiles not found via reflection.");
        return (Dictionary<string, CohabitationProfile>)(field.GetValue(null)
            ?? throw new InvalidOperationException("CohabitationCatalog._profiles was null."));
    }
}
