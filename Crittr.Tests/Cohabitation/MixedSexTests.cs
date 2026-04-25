using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class MixedSexTests
{
    [Fact]
    public void Male_female_pair_of_one_male_species_has_no_gender_conflict()
    {
        var leopardGecko = TestData.Species("Leopard Gecko", SpeciesType.Reptile);
        var resident = TestData.Resident(leopardGecko, "She", sex: "Female");

        var result = CohabitationChecker.Check(leopardGecko, incomingSex: "Male", new[] { resident });

        result.CanCohabit.Should().BeTrue();
        result.HasWarnings.Should().BeFalse();
        result.Conflicts.Should().BeEmpty();
    }
}
