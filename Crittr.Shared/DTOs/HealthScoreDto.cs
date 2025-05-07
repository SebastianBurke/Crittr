namespace Crittr.Shared.DTOs;

public class HealthScoreDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public string CritterName { get; set; } = string.Empty;
    public DateTime AssessmentDate { get; set; }
    public int Score { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string? AssessedBy { get; set; }
}
