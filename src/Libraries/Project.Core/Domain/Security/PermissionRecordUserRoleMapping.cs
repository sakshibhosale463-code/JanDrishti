namespace Project.Core.Domain.Security;

/// <summary>
/// Represents a permission record-customer role mapping class
/// </summary>
public partial class PermissionRecordUserRoleMapping : BaseEntity
{
    /// <summary>
    /// Gets or sets the permission record identifier
    /// </summary>
    public long PermissionRecordId { get; set; }

    /// <summary>
    /// Gets or sets the user role identifier
    /// </summary>
    public long UserRoleId { get; set; }
}