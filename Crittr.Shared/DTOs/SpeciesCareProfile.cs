using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.DTOs;

public class SpeciesCareProfile
{
    /// <summary>How often this species typically needs feeding (in days). Adults.</summary>
    [Range(0, 365)]
    public int FeedingFrequencyDays { get; set; } = 1;

    /// <summary>Human-readable feeding guidance (age variation, prey type, etc.)</summary>
    [StringLength(2000)]
    public string? FeedingNotes { get; set; }

    /// <summary>Approximate days between shedding cycles. Null for species that don't shed (fish, birds, mammals, tortoises).</summary>
    [Range(0, 3650)]
    public int? SheddingIntervalDays { get; set; }

    [Range(-50, 100)]
    public double IdealTempMinC { get; set; }
    [Range(-50, 100)]
    public double IdealTempMaxC { get; set; }
    [Range(0, 100)]
    public double IdealHumidityMin { get; set; }
    [Range(0, 100)]
    public double IdealHumidityMax { get; set; }

    public bool IsNocturnal { get; set; }

    /// <summary>True for species known to fast for extended periods without it being a health concern (e.g. ball pythons, tarantulas before molts).</summary>
    public bool NaturalFastingNormal { get; set; }

    /// <summary>Curated list of accepted foods for this species, shown as quick-pick chips in the feeding form.</summary>
    public IReadOnlyList<string> AcceptedFoods { get; set; } = Array.Empty<string>();
}
