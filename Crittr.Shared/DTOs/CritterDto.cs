using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class CritterDto
{
    public int Id { get; set; }
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;
    [StringLength(120)]
    public string Species { get; set; } = string.Empty;
    [StringLength(500)]
    public string IconUrl { get; set; } = string.Empty!;
    public DateTime DateAcquired { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [StringLength(40)]
    public string? Sex { get; set; }

    [Range(0, 200000)]
    public double? Weight { get; set; }
    [Range(0, 1000)]
    public double? Length { get; set; }
    [StringLength(2000)]
    public string? Description { get; set; }

    public int? EnclosureProfileId { get; set; }
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    // Summary data (dashboard use)
    public SpeciesType SpeciesType { get; set; }
    public DateTime? LastFeedingDate { get; set; }
    public DateTime? LastWeightDate { get; set; }
    public DateTime? LastSheddingDate { get; set; }
    [Range(0, 100)]
    public int? RecentHealthScore { get; set; }
    [Range(0, 10000)]
    public int PendingTasksCount { get; set; }
    public CritterCondition Condition { get; set; } = CritterCondition.Unknown;
}
