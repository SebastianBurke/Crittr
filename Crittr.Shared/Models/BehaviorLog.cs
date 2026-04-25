using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Models;

public class BehaviorLog
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime LogDate { get; set; }
    public BehaviorType BehaviorType { get; set; }
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
    public bool IsAbnormal { get; set; }
}
