using Crittr.Shared.Models.Enums;

namespace Crittr.Shared.DTOs;

public class ScheduledTaskDto
{
    public int Id { get; set; }
    public int CritterId { get; set; }
    public string CritterName { get; set; } = string.Empty;
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