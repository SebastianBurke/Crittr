using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class FeedingRecordDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    [StringLength(120)]
    public string CritterName { get; set; } = string.Empty;
    public DateTime FeedingDate { get; set; }
    [StringLength(80)]
    public string FoodItem { get; set; } = string.Empty;
    [Range(0, 10000)]
    public double Quantity { get; set; }
    public ItemType ItemType { get; set; }
    public FeedingUnit FeedingUnit { get; set; } = FeedingUnit.Items;
    public string ItemTypeString => ItemType.ToString();
    public bool WasEaten { get; set; }
    [StringLength(2000)]
    public string? Notes { get; set; }
}
