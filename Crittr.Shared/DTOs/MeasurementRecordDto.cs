using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.DTOs;

public class MeasurementRecordDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    [StringLength(120)]
    public string CritterName { get; set; } = string.Empty;
    public DateTime MeasurementDate { get; set; }
    [Range(0, 200000)]
    public double Weight { get; set; }
    [Range(0, 1000)]
    public double Length { get; set; }
    [StringLength(2000)]
    public string? Notes { get; set; }
    [Range(-200000, 200000)]
    public double? WeightChange { get; set; }
    [Range(-1000, 1000)]
    public double? LengthChange { get; set; }
}
