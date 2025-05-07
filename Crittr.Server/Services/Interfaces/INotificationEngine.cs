namespace Crittr.Server.Services.Interfaces;

public interface INotificationEngine
{
    Task SendTaskReminderAsync(int taskId);
    Task SendFeedingReminderAsync(int critterId);
    Task SendHealthAlertAsync(int critterId, string message);
    Task SendEnvironmentalAlertAsync(int critterId, string message);
    Task<List<string>> GetPendingNotificationsAsync(string userId);
    Task MarkNotificationAsReadAsync(int notificationId);
}