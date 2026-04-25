using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.DTOs;

public class EnvironmentalReadingDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    [StringLength(120)]
    public string CritterName { get; set; } = string.Empty;
    public DateTime ReadingDate { get; set; }
    [Range(-20, 200)]
    public double Temperature { get; set; }
    [Range(0, 100)]
    public double Humidity { get; set; }
    [Range(0, 20)]
    public double? UVBIndex { get; set; }
    public bool IsManualReading { get; set; }
    [StringLength(80)]
    public string? Source { get; set; }
}
