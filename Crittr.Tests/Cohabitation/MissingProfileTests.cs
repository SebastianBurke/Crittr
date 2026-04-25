using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class MissingProfileTests
{
    [Fact]
    public void Incoming_with_null_CohabProfile_blocks()
    {
        var unknown = TestData.Synthetic("Unknown Species", SpeciesType.Reptile, profile: null);
        var leopardGecko = TestData.Species("Leopard Gecko", SpeciesType.Reptile);
        var resident = TestData.Resident(leopardGecko, "Spots", sex: "Female");

        var result = CohabitationChecker.Check(unknown, incomingSex: "Female", new[] { resident });

        result.CanCohabit.Should().BeFalse();
        result.Conflicts.Should().ContainSingle()
            .Which.IsHardBlock.Should().BeTrue();
    }

    [Fact]
    public void Resident_with_null_CohabProfile_blocks()
    {
        var leopardGecko = TestData.Species("Leopard Gecko", SpeciesType.Reptile);
        var unknown = TestData.Synthetic("Unknown Species", SpeciesType.Reptile, profile: null);
        var resident = TestData.Resident(unknown, "Mystery", sex: null);

        var result = CohabitationChecker.Check(leopardGecko, incomingSex: "Female", new[] { resident });

        result.CanCohabit.Should().BeFalse();
        result.Conflicts.Should().ContainSingle()
            .Which.IsHardBlock.Should().BeTrue();
    }
}
