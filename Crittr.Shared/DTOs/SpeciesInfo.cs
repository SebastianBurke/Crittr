using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class SpeciesInfo
{
    [StringLength(120)]
    public string CommonName { get; set; } = default!;
    [StringLength(120)]
    public string ScientificName { get; set; } = default!;
    public SpeciesType Type { get; set; }
    [StringLength(500)]
    public string IconUrl { get; set; } = default!;
    public List<EnclosureType> CompatibleEnclosureTypes { get; set; } = new();
    public SpeciesCareProfile? CareProfile { get; set; }
    public CohabitationProfile? CohabProfile { get; set; }
}
