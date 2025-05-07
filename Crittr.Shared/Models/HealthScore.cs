namespace Crittr.Shared.Models;

public class HealthScore
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime AssessmentDate { get; set; }
    public int Score { get; set; } // Scale of 1-10
    public string Notes { get; set; } = string.Empty;
    public string? AssessedBy { get; set; }
}