namespace Project.Core.Domain.Users;

/// <summary>
/// Represents user permission record mapping
/// </summary>
public partial class UserPermissionRecordMapping : BaseEntity
{
    /// <summary>
    /// Gets or sets the permission record identifier
    /// </summary>
    public long PermissionRecordId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier
    /// </summary>
    public long UserId { get; set; }
}
