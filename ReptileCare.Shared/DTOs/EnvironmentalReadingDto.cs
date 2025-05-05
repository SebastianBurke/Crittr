namespace ReptileCare.Shared.DTOs;

public class EnvironmentalReadingDto
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public string ReptileName { get; set; } = string.Empty;
    public DateTime ReadingDate { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double? UVBIndex { get; set; }
    public bool IsManualReading { get; set; }
    public string? Source { get; set; }
}