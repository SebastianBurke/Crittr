using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class PredatoryTests
{
    [Fact]
    public void Predatory_incoming_warns_against_a_compatible_resident()
    {
        var ackie = TestData.Species("Ackie Monitor", SpeciesType.Reptile);
        var resident = TestData.Resident(ackie, "Spike", sex: "Female");

        var result = CohabitationChecker.Check(ackie, incomingSex: "Female", new[] { resident });

        result.CanCohabit.Should().BeTrue("predatory triggers Warn, not Block");
        result.HasWarnings.Should().BeTrue();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("predatory");
    }

    [Fact]
    public void Predatory_resident_warns_against_a_compatible_incoming()
    {
        var ferret = TestData.Species("Ferret", SpeciesType.Mammal);
        var rat = TestData.Species("Fancy Rat", SpeciesType.Mammal);
        var resident = TestData.Resident(ferret, "Bandit", sex: "Female");

        var result = CohabitationChecker.Check(rat, incomingSex: "Female", new[] { resident });

        result.HasWarnings.Should().BeTrue();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("predatory");
    }
}
