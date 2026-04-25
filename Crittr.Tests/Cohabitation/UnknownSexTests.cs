using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class UnknownSexTests
{
    [Fact]
    public void Male_incoming_against_unknown_sex_resident_warns_for_one_male_species()
    {
        var leopardGecko = TestData.Species("Leopard Gecko", SpeciesType.Reptile);
        var resident = TestData.Resident(leopardGecko, "Mystery", sex: null);

        var result = CohabitationChecker.Check(leopardGecko, incomingSex: "Male", new[] { resident });

        result.CanCohabit.Should().BeTrue();
        result.HasWarnings.Should().BeTrue();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("unknown");
    }

    [Fact]
    public void Unknown_sex_incoming_against_male_resident_warns_for_one_male_species()
    {
        var leopardGecko = TestData.Species("Leopard Gecko", SpeciesType.Reptile);
        var resident = TestData.Resident(leopardGecko, "Stud", sex: "Male");

        var result = CohabitationChecker.Check(leopardGecko, incomingSex: "Unknown", new[] { resident });

        result.CanCohabit.Should().BeTrue();
        result.HasWarnings.Should().BeTrue();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("unknown");
    }
}
