namespace Crittr.Shared.Models;

public class EnvironmentalReading
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime ReadingDate { get; set; }
    public double Temperature { get; set; } // in Celsius
    public double Humidity { get; set; } // in percentage
    public double? UVBIndex { get; set; }
    public bool IsManualReading { get; set; }
    public string? Source { get; set; } // e.g., "Herpstat", "Manual"
}