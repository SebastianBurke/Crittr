using ReptileCare.Shared.Models.Enums;

namespace ReptileCare.Shared.DTOs;

public class ScheduledTaskDto
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public string ReptileName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public TaskPriority Priority { get; set; }
    public string PriorityString => Priority.ToString();
    public DateTime? CompletedDate { get; set; }
    public bool IsOverdue => !IsCompleted && DueDate < DateTime.Now;
    public int DaysUntilDue => IsCompleted ? 0 : (DueDate - DateTime.Now).Days;
}