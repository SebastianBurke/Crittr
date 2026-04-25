using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class CrossTypeTests
{
    [Fact]
    public void Different_animal_types_warn_when_neither_is_community()
    {
        var crestedGecko = TestData.Species("Crested Gecko", SpeciesType.Reptile);
        var cockatiel = TestData.Species("Cockatiel", SpeciesType.Bird);
        var resident = TestData.Resident(cockatiel, "Pip", sex: "Female");

        var result = CohabitationChecker.Check(crestedGecko, incomingSex: "Female", new[] { resident });

        result.CanCohabit.Should().BeTrue();
        result.HasWarnings.Should().BeTrue();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("different animal types");
    }

    [Fact]
    public void Cross_type_warning_is_suppressed_when_either_side_is_community()
    {
        var goldfish = TestData.Species("Goldfish", SpeciesType.Fish);
        var crestedGecko = TestData.Species("Crested Gecko", SpeciesType.Reptile);
        var resident = TestData.Resident(crestedGecko, "Geck", sex: "Female");

        var result = CohabitationChecker.Check(goldfish, incomingSex: "Female", new[] { resident });

        result.HasWarnings.Should().BeFalse("Goldfish is Community so the cross-type warning is exempted");
        result.Conflicts.Should().BeEmpty();
    }
}
