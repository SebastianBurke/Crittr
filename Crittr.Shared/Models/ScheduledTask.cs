using System.ComponentModel.DataAnnotations;
using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Models;

public class ScheduledTask
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? CompletedDate { get; set; }
}
