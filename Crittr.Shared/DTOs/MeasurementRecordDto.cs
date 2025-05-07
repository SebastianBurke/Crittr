namespace Crittr.Shared.DTOs;

public class MeasurementRecordDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public string CritterName { get; set; } = string.Empty;
    public DateTime MeasurementDate { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }
    public string? Notes { get; set; }
    public double? WeightChange { get; set; } // From previous measurement
    public double? LengthChange { get; set; } // From previous measurement
}