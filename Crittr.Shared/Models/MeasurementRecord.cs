namespace Crittr.Shared.Models;

public class MeasurementRecord
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime MeasurementDate { get; set; }
    public double Weight { get; set; } // in grams
    public double Length { get; set; } // in centimeters
    public string? Notes { get; set; }
}