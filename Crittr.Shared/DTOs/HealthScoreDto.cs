using System.ComponentModel.DataAnnotations;

namespace Crittr.Shared.DTOs;

public class HealthScoreDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    [StringLength(120)]
    public string CritterName { get; set; } = string.Empty;
    public DateTime AssessmentDate { get; set; }
    [Range(0, 100)]
    public int Score { get; set; }
    [StringLength(2000)]
    public string Notes { get; set; } = string.Empty;
    [StringLength(120)]
    public string? AssessedBy { get; set; }
}
