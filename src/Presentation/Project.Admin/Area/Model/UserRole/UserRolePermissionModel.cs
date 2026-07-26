using Project.Web.Framework.Models;

namespace Project.Admin.Area.Model.UserRole;

public partial record UserRolePermissionModel: BaseModel
{
    #region Constructor

    public UserRolePermissionModel()
    {
        PermissionIds = new List<long>();
        Permissions = new List<SelectListModel>();
        AvailablePermissions = new List<SelectListModel>();
    }

    #endregion

    #region Properties
    public string Name { get; set; }
    public string SystemName { get; set; }
    public string Category { get; set; }
    public bool Active { get; set; }
    public long UserRoleId { get; set; }
    public List<long> PermissionIds { get; set; }
    public IList<SelectListModel> Permissions { get; set; }
    public IList<SelectListModel> AvailablePermissions { get; set; }

    #endregion
}
