using ReptileCare.Shared.Models.Enums;

namespace ReptileCare.Shared.DTOs;

public class EnclosureProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public string? SubstrateType { get; set; }
    public bool HasUVBLighting { get; set; }
    public bool HasHeatingElement { get; set; }
    public double IdealTemperature { get; set; }
    public double IdealHumidity { get; set; }

    public EnclosureType EnclosureType { get; set; }

    public List<int> AssignedReptileIds { get; set; } = new();
}