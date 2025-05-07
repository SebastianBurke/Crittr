using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Models;

public class FeedingRecord
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public DateTime FeedingDate { get; set; }
    public string FoodItem { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public ItemType ItemType { get; set; }
    public bool WasEaten { get; set; }
    public string? Notes { get; set; }
}