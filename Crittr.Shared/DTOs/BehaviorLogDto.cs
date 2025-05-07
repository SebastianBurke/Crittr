using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class BehaviorLogDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public string CritterName { get; set; } = string.Empty;
    public DateTime LogDate { get; set; }
    public BehaviorType BehaviorType { get; set; }
    public string BehaviorTypeString => BehaviorType.ToString();
    public string Description { get; set; } = string.Empty;
    public bool IsAbnormal { get; set; }
}
