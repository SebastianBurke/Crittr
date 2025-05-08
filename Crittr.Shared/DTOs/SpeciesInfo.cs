using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class SpeciesInfo
{
    public string CommonName { get; set; } = default!;
    public string ScientificName { get; set; } = default!;
    public SpeciesType Type { get; set; }
    public string IconUrl { get; set; } = default!;
    public List<EnclosureType> CompatibleEnclosureTypes { get; set; } = new();
}