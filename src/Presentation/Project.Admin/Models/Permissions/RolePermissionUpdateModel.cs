namespace Project.Admin.Models.Permissions;

public partial record RolePermissionUpdateModel
{
    public List<long> PermissionIds { get; set; }
    public long RoleId { get; set; }
}
