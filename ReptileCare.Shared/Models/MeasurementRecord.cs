namespace ReptileCare.Shared.Models;

public class MeasurementRecord
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public Reptile? Reptile { get; set; }
    public DateTime MeasurementDate { get; set; }
    public double Weight { get; set; } // in grams
    public double Length { get; set; } // in centimeters
    public string? Notes { get; set; }
}