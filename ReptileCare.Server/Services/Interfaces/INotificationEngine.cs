namespace ReptileCare.Server.Services.Interfaces;

public interface INotificationEngine
{
    Task SendTaskReminderAsync(int taskId);
    Task SendFeedingReminderAsync(int reptileId);
    Task SendHealthAlertAsync(int reptileId, string message);
    Task SendEnvironmentalAlertAsync(int reptileId, string message);
    Task<List<string>> GetPendingNotificationsAsync(string userId);
    Task MarkNotificationAsReadAsync(int notificationId);
}