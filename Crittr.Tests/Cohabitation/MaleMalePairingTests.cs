using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class MaleMalePairingTests
{
    [Theory]
    [InlineData("Leopard Gecko", SpeciesType.Reptile)]
    [InlineData("Crested Gecko", SpeciesType.Reptile)]
    [InlineData("Gargoyle Gecko", SpeciesType.Reptile)]
    [InlineData("African Fat-tailed Gecko", SpeciesType.Reptile)]
    [InlineData("Canary", SpeciesType.Bird)]
    [InlineData("African Cichlid", SpeciesType.Fish)]
    public void Two_males_block_when_male_male_is_lethal(string commonName, SpeciesType type)
    {
        var species = TestData.Species(commonName, type);
        var resident = TestData.Resident(species, "Resident", sex: "Male");

        var result = CohabitationChecker.Check(species, incomingSex: "Male", new[] { resident });

        result.CanCohabit.Should().BeFalse($"two male {commonName}s with MaleMaleIsLethal=true must Block");
        result.Conflicts.Should().ContainSingle()
            .Which.IsHardBlock.Should().BeTrue();
    }

    [Theory]
    [InlineData("Green Iguana", SpeciesType.Reptile)]
    [InlineData("Chinese Water Dragon", SpeciesType.Reptile)]
    [InlineData("Uromastyx", SpeciesType.Reptile)]
    [InlineData("Cockatiel", SpeciesType.Bird)]
    [InlineData("Indian Ringneck Parakeet", SpeciesType.Bird)]
    public void Two_males_warn_when_male_male_is_not_lethal(string commonName, SpeciesType type)
    {
        var species = TestData.Species(commonName, type);
        var resident = TestData.Resident(species, "Resident", sex: "Male");

        var result = CohabitationChecker.Check(species, incomingSex: "Male", new[] { resident });

        result.CanCohabit.Should().BeTrue($"two male {commonName}s with MaleMaleIsLethal=false must Warn, not Block");
        result.HasWarnings.Should().BeTrue();
        result.Conflicts.Should().ContainSingle()
            .Which.IsHardBlock.Should().BeFalse();
    }
}
