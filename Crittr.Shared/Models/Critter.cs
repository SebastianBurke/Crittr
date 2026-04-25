using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Models;

public class Critter
{
    public int Id { get; set; }
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;
    [StringLength(120)]
    public string Species { get; set; } = string.Empty;
    public SpeciesType SpeciesType { get; set; }

    [StringLength(500)]
    public string IconUrl { get; set; } = "img/critters/default.svg";
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
    public EnclosureProfile? EnclosureProfile { get; set; }

    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    // Navigation
    public ICollection<FeedingRecord> FeedingRecords { get; set; } = new List<FeedingRecord>();
    public ICollection<EnvironmentalReading> EnvironmentalReadings { get; set; } = new List<EnvironmentalReading>();
    public ICollection<BehaviorLog> BehaviorLogs { get; set; } = new List<BehaviorLog>();
    public ICollection<SheddingRecord> SheddingRecords { get; set; } = new List<SheddingRecord>();
    public ICollection<MeasurementRecord> MeasurementRecords { get; set; } = new List<MeasurementRecord>();
    public ICollection<ScheduledTask> ScheduledTasks { get; set; } = new List<ScheduledTask>();
    public ICollection<HealthScore> HealthScores { get; set; } = new List<HealthScore>();
}
