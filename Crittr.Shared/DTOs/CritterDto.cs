using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class CritterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty!;
    public DateTime DateAcquired { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Sex { get; set; }
    
    public double? Weight { get; set; }
    public double? Length { get; set; }
    public string? Description { get; set; }
    
    public int? EnclosureProfileId { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Summary data (dashboard use)
    public SpeciesType SpeciesType { get; set; }
    public DateTime? LastFeedingDate { get; set; }
    public DateTime? LastWeightDate { get; set; }
    public DateTime? LastSheddingDate { get; set; }
    public int? RecentHealthScore { get; set; }
    public int PendingTasksCount { get; set; }
}