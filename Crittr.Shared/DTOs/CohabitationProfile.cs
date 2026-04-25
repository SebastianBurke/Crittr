using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class CohabitationProfile
{
    public SocialStructure SocialNeeds { get; set; }
    public bool IsTerritorial { get; set; }
    public bool IsPredatory { get; set; }
    public string[] IncompatibleWith { get; set; } = [];
    [StringLength(2000)]
    public string? CohabNote { get; set; }
    public GenderPairingRule GenderRule { get; set; } = GenderPairingRule.AnyGenderOk;
    public bool MaleMaleIsLethal { get; set; } = false;
}
