using Crittr.Shared.DTOs;
using Crittr.Shared.Models.Enums;
using Crittr.Shared.Utilities;
using FluentAssertions;
using Xunit;

namespace Crittr.Tests.Cohabitation;

public class IncompatibleWithTests
{
    // Synthetic data: every catalog species that has IncompatibleWith populated (only King Snake)
    // is also SoloOnly, which short-circuits before the IncompatibleWith branch can fire. We exercise
    // the branch with synthetic profiles to lock down behavior for the future.
    [Fact]
    public void Explicit_incompatible_with_blocks_a_pair_that_would_otherwise_be_compatible()
    {
        var alpha = TestData.Synthetic("Alpha", SpeciesType.Reptile, new CohabitationProfile
        {
            SocialNeeds = SocialStructure.PairsOk,
            IncompatibleWith = new[] { "Beta" },
        });
        var beta = TestData.Synthetic("Beta", SpeciesType.Reptile, new CohabitationProfile
        {
            SocialNeeds = SocialStructure.PairsOk,
        });
        var resident = TestData.Resident(beta, "Bee", sex: "Female");

        var result = CohabitationChecker.Check(alpha, incomingSex: "Female", new[] { resident });

        result.CanCohabit.Should().BeFalse();
        result.Conflicts.Should().ContainSingle()
            .Which.Reason.Should().Contain("explicitly incompatible");
    }
}
