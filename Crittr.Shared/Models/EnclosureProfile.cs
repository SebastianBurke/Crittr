namespace Crittr.Shared.Models;

public class EnclosureProfile
{
    public int Id { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Length { get; set; } // in centimeters
    public double Width { get; set; } // in centimeters
    public double Height { get; set; } // in centimeters
    public string? SubstrateType { get; set; }
    public bool HasUVBLighting { get; set; }
    public bool HasHeatingElement { get; set; }
    public double IdealTemperature { get; set; } // in Celsius
    public double IdealHumidity { get; set; } // in percentage
    
    // Navigation properties
    public ICollection<Critter> Critters { get; set; } = new List<Critter>();
    public ICollection<CaregiverAccess> Caregivers { get; set; } = new List<CaregiverAccess>();
}
