using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class FeedingRecordDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public string CritterName { get; set; } = string.Empty;
    public DateTime FeedingDate { get; set; }
    public string FoodItem { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public ItemType ItemType { get; set; }
    public string ItemTypeString => ItemType.ToString();
    public bool WasEaten { get; set; }
    public string? Notes { get; set; }
}