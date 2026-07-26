using Project.Core.Domain.Common;

namespace Project.Core.Domain.Users;

/// <summary>
/// Represent user of system
/// </summary>
public partial class User : BaseEntity, ISoftDeletedEntity
{
    /// <summary>
    /// Gets or sets the name of user
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address
    /// </summary>
    public string EmailAddress { get; set; }

    /// <summary>
    /// Gets or sets the code
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the mobile number
    /// </summary>
    public string MobileNumber { get; set; }
   
    /// <summary>
    /// Gets or sets the password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the value whether user is active or not
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the user role identifier
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity has been deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Gets or sets the created date time
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the updated date time
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }

    public string ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public string UserCode { get; set; }
}