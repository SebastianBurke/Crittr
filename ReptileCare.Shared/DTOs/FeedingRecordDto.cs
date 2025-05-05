using ReptileCare.Shared.Models.Enums;

namespace ReptileCare.Shared.DTOs;

public class FeedingRecordDto
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public string ReptileName { get; set; } = string.Empty;
    public DateTime FeedingDate { get; set; }
    public string FoodItem { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public ItemType ItemType { get; set; }
    public string ItemTypeString => ItemType.ToString();
    public bool WasEaten { get; set; }
    public string? Notes { get; set; }
}