namespace Project.Admin.Models.Permissions;

public partial record UserPermissionUpdateModel
{
    public List<long> PermissionIds { get; set; }
    public long UserId { get; set; }
}
