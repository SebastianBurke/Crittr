using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.Models;

public class ScheduledTask
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public Critter? Critter { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? CompletedDate { get; set; }
}
