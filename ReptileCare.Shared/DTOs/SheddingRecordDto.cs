namespace ReptileCare.Shared.DTOs;

public class SheddingRecordDto
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public string ReptileName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool IsComplete { get; set; }
    public bool WasAssisted { get; set; }
    public string? Notes { get; set; }
    public int DurationDays => CompletionDate.HasValue ? (CompletionDate.Value - StartDate).Days : (DateTime.Now - StartDate).Days;
}