namespace Crittr.Shared.Models;

public class SheddingRecord
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool IsComplete { get; set; }
    public bool WasAssisted { get; set; }
    public string? Notes { get; set; }
}