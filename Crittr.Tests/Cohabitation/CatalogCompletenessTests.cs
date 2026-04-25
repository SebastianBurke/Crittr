using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class CatalogCompletenessTests
{
    public static IEnumerable<object[]> AllSpeciesNames()
        => TestData.AllCatalogEntries().Select(kvp => new object[] { kvp.Key });

    [Theory]
    [MemberData(nameof(AllSpeciesNames))]
    public void Every_species_in_catalog_has_a_non_null_CohabProfile(string commonName)
    {
        var entries = TestData.AllCatalogEntries();
        entries.Should().ContainKey(commonName);
        entries[commonName].Should().NotBeNull(
            $"missing CohabProfile for '{commonName}' would silently block all cohabitation since the new behavior is BLOCK on null.");
    }
}
