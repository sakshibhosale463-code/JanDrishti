using Project.Core;

namespace Project.Services.UserRoles.EntityMode;
public class PermissionRecordEntityModel : BaseEntity
{
    public string Name { get; set; }
    public string SystemName { get; set; }
    public string Category { get; set; }
    public bool Active { get; set; }
}
