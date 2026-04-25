using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.Models;

public class EnvironmentalReading
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime ReadingDate { get; set; }
    [Range(-20, 200)]
    public double Temperature { get; set; } // in Celsius
    [Range(0, 100)]
    public double Humidity { get; set; } // in percentage
    [Range(0, 20)]
    public double? UVBIndex { get; set; }
    public bool IsManualReading { get; set; }
    [StringLength(80)]
    public string? Source { get; set; } // e.g., "Herpstat", "Manual"
}
