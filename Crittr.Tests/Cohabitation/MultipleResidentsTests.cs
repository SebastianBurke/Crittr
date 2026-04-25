using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class MultipleResidentsTests
{
    [Fact]
    public void Aggregates_conflicts_across_multiple_residents_and_blocks_if_any_is_a_hard_block()
    {
        var crestedGecko = TestData.Species("Crested Gecko", SpeciesType.Reptile);
        var cockatiel = TestData.Species("Cockatiel", SpeciesType.Bird);
        var goldfish = TestData.Species("Goldfish", SpeciesType.Fish);

        var residents = new[]
        {
            TestData.Resident(cockatiel,    "Birdie", sex: "Male"),    // cross-type warn
            TestData.Resident(crestedGecko, "Geck",   sex: "Male"),    // same-species lethal male/male block
            TestData.Resident(goldfish,     "Bubbles", sex: "Female"), // Community, exempt from cross-type
        };

        var result = CohabitationChecker.Check(crestedGecko, incomingSex: "Male", residents);

        result.CanCohabit.Should().BeFalse("at least one conflict is a hard block");
        result.HasWarnings.Should().BeTrue();
        result.Conflicts.Should().HaveCount(2);
        result.Conflicts.Should().Contain(c => c.IsHardBlock);
        result.Conflicts.Should().NotContain(c => c.CritterName == "Bubbles");
    }
}
