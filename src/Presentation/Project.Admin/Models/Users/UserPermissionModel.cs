using Project.Core;
using Project.Web.Framework.Models;

namespace Project.Admin.Models.Users;

/// <summary>
/// Represents user permission model
/// </summary>
public class UserPermissionModel : BaseEntity
{
    #region Constructor

    public UserPermissionModel()
    {
        Permissions = new List<SelectListModel>();
        AvailablePermissions = new List<SelectListModel>();
    }

    #endregion

    #region Properties

    public IList<SelectListModel> Permissions { get; set; }
    public IList<SelectListModel> AvailablePermissions { get; set; }

    #endregion
}
