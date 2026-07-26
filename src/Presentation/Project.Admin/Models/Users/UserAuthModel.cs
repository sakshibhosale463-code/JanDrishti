using Project.Web.Framework.Models;

namespace Project.Admin.Models.Users;

/// <summary>
/// Represents user auth model
/// </summary>
public partial record UserAuthModel : BaseModel
{
    #region Constructor

    public UserAuthModel()
    {
        
        Permissions = new List<string>();
    }

    #endregion

    #region Properties

    public string Name { get; set; }
    public string Token { get; set; }
    public string EmailAddress { get; set; }
    public string MobileNumber { get; set; }
    public string UserName { get; set; }
    public bool Active { get; set; }
    public long RoleId { get; set; }
    public string Password { get; set; }
    public string Remark { get; set; }
    public string RoleName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public IList<string> Permissions { get; set; }
    public bool MultiDeviceLoginEnabled { get; set; }

    #endregion
}
