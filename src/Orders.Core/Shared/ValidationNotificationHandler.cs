using System.Collections.Generic;

namespace Orders.Core.Shared;

public class ValidationNotificationHandler
{
    private readonly List<ValidationNotification> _notifications = new();
    public virtual IReadOnlyCollection<ValidationNotification> Notifications => _notifications.AsReadOnly();

    public void AddNotification(
        string code,
        string message
    )
    {
        var notification = new ValidationNotification(
            code,
            message
        );

        _notifications.Add(notification);
    }
}