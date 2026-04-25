using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.DTOs;

public class SheddingRecordDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    [StringLength(120)]
    public string CritterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool IsComplete { get; set; }
    public bool WasAssisted { get; set; }
    [StringLength(2000)]
    public string? Notes { get; set; }
    public int DurationDays => CompletionDate.HasValue ? (CompletionDate.Value - StartDate).Days : (DateTime.Now - StartDate).Days;
}
