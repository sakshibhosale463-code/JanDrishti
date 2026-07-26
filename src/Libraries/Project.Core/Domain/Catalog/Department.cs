using Project.Core.Domain.Common;

namespace Project.Core.Domain.Catalog;

/// <summary>
/// Represents department
/// </summary>
public partial class Department : BaseEntity, ISoftDeletedEntity
{
    /// <summary>
    /// Gets or sets the department code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user role is active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the remark
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity has been deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Gets or sets the created utc date time
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the updated utc date time
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }
}
