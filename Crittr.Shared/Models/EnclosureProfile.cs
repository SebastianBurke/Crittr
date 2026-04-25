using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Models;

public class EnclosureProfile
{
    public int Id { get; set; }
    [StringLength(450)]
    public string OwnerId { get; set; } = string.Empty;
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;
    public EnclosureType EnclosureType { get; set; } = EnclosureType.Terrarium;
    [Range(0.1, 600)]
    public double Length { get; set; } // in centimeters
    [Range(0.1, 600)]
    public double Width { get; set; } // in centimeters
    [Range(0.1, 600)]
    public double Height { get; set; } // in centimeters
    [StringLength(80)]
    public string? SubstrateType { get; set; }
    public bool HasUVBLighting { get; set; }
    public bool HasHeatingElement { get; set; }
    [Range(-20, 200)]
    public double IdealTemperature { get; set; } // in Celsius
    [Range(0, 100)]
    public double IdealHumidity { get; set; } // in percentage

    // Navigation properties
    public ICollection<Critter> Critters { get; set; } = new List<Critter>();
    public ICollection<CaregiverAccess> Caregivers { get; set; } = new List<CaregiverAccess>();
}
