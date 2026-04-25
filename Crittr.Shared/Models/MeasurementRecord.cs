using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.Models;

public class MeasurementRecord
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime MeasurementDate { get; set; }
    [Range(0, 200000)]
    public double Weight { get; set; } // in grams
    [Range(0, 1000)]
    public double Length { get; set; } // in centimeters
    [StringLength(2000)]
    public string? Notes { get; set; }
}
