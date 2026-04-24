namespace Crittr.Shared.DTOs;

public class SpeciesCareProfile
{
    /// <summary>How often this species typically needs feeding (in days). Adults.</summary>
    public int FeedingFrequencyDays { get; set; } = 1;

    /// <summary>Human-readable feeding guidance (age variation, prey type, etc.)</summary>
    public string? FeedingNotes { get; set; }

    /// <summary>Approximate days between shedding cycles. Null for species that don't shed (fish, birds, mammals, tortoises).</summary>
    public int? SheddingIntervalDays { get; set; }

    public double IdealTempMinC { get; set; }
    public double IdealTempMaxC { get; set; }
    public double IdealHumidityMin { get; set; }
    public double IdealHumidityMax { get; set; }

    public bool IsNocturnal { get; set; }

    /// <summary>True for species known to fast for extended periods without it being a health concern (e.g. ball pythons, tarantulas before molts).</summary>
    public bool NaturalFastingNormal { get; set; }

    /// <summary>Curated list of accepted foods for this species, shown as quick-pick chips in the feeding form.</summary>
    public IReadOnlyList<string> AcceptedFoods { get; set; } = Array.Empty<string>();
}
