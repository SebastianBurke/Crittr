using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class SoloOnlyTests
{
    [Fact]
    public void Two_ball_pythons_are_blocked_with_must_live_alone_reason()
    {
        var ballPython = TestData.Species("Ball Python", SpeciesType.Reptile);
        var resident = TestData.Resident(ballPython, "Monty", sex: null);

        var result = CohabitationChecker.Check(ballPython, incomingSex: null, new[] { resident });

        result.CanCohabit.Should().BeFalse();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("must live alone");
    }

    [Fact]
    public void Incoming_solo_only_blocks_against_a_social_resident()
    {
        var ballPython = TestData.Species("Ball Python", SpeciesType.Reptile);
        var leopardGecko = TestData.Species("Leopard Gecko", SpeciesType.Reptile);
        var resident = TestData.Resident(leopardGecko, "Spots", sex: "Female");

        var result = CohabitationChecker.Check(ballPython, incomingSex: null, new[] { resident });

        result.CanCohabit.Should().BeFalse();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("Ball Python must live alone");
    }

    [Fact]
    public void Resident_solo_only_blocks_against_a_social_incoming()
    {
        var leopardGecko = TestData.Species("Leopard Gecko", SpeciesType.Reptile);
        var ballPython = TestData.Species("Ball Python", SpeciesType.Reptile);
        var resident = TestData.Resident(ballPython, "Monty", sex: null);

        var result = CohabitationChecker.Check(leopardGecko, incomingSex: "Female", new[] { resident });

        result.CanCohabit.Should().BeFalse();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("must live alone");
    }
}
