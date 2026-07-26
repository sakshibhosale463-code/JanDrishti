using Project.Core;

namespace Project.Core.Domain.Master;

public class NotificationMaster : BaseEntity
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string NotificationType { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
