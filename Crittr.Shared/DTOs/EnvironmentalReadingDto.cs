namespace Crittr.Shared.DTOs;

public class EnvironmentalReadingDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public string CritterName { get; set; } = string.Empty;
    public DateTime ReadingDate { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double? UVBIndex { get; set; }
    public bool IsManualReading { get; set; }
    public string? Source { get; set; }
}