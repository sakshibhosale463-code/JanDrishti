using Project.Core;

namespace Project.Services.Users.EntityModel;

/// <summary>
/// Represents user entity model
/// </summary>
public class UserEntityModel : BaseEntity
{
    #region Properties

    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public string MobileNumber { get; set; }
    public string Code { get; set; }
    public bool Active { get; set; }
    public string RoleName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Locations { get; set; }
    public string Departments { get; set; }
    public string Remark { get; set; }
    public bool MultiDeviceLoginEnabled { get; set; }

    #endregion
}
