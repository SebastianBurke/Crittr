using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class EnclosureProfileDto
{
    public int Id { get; set; }
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;
    [Range(0.1, 600)]
    public double Length { get; set; }
    [Range(0.1, 600)]
    public double Width { get; set; }
    [Range(0.1, 600)]
    public double Height { get; set; }
    [StringLength(80)]
    public string? SubstrateType { get; set; }
    public bool HasUVBLighting { get; set; }
    public bool HasHeatingElement { get; set; }
    [Range(-20, 200)]
    public double IdealTemperature { get; set; }
    [Range(0, 100)]
    public double IdealHumidity { get; set; }

    public EnclosureType EnclosureType { get; set; }

    public List<int> AssignedCritterIds { get; set; } = new();
}
