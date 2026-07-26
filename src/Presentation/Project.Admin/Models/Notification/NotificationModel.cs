using Project.Web.Framework.Models;

namespace Project.Admin.Models.Notification;

public record class NotificationModel : BaseModel
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string NotificationType { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
