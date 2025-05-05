using ReptileCare.Shared.Models.Enums;

namespace ReptileCare.Shared.Models;

public class FeedingRecord
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public Reptile? Reptile { get; set; }
    public DateTime FeedingDate { get; set; }
    public string FoodItem { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public ItemType ItemType { get; set; }
    public bool WasEaten { get; set; }
    public string? Notes { get; set; }
}