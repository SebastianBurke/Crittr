using ReptileCare.Shared.Models.Enums;

namespace ReptileCare.Shared.DTOs;

public class BehaviorLogDto
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public string ReptileName { get; set; } = string.Empty;
    public DateTime LogDate { get; set; }
    public BehaviorType BehaviorType { get; set; }
    public string BehaviorTypeString => BehaviorType.ToString();
    public string Description { get; set; } = string.Empty;
    public bool IsAbnormal { get; set; }
}
