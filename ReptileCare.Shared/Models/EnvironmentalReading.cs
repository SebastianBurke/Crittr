namespace ReptileCare.Shared.Models;

public class EnvironmentalReading
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public Reptile? Reptile { get; set; }
    public DateTime ReadingDate { get; set; }
    public double Temperature { get; set; } // in Celsius
    public double Humidity { get; set; } // in percentage
    public double? UVBIndex { get; set; }
    public bool IsManualReading { get; set; }
    public string? Source { get; set; } // e.g., "Herpstat", "Manual"
}