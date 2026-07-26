using Project.Core;
using Project.Web.Framework.Models;

namespace Project.Admin.Models.Users;

public class CategoryWisePermissionModel
{
    public string Category { get; set; }
    public List<SelectListModel> Permissions { get; set; } = new List<SelectListModel>();
}

public class UserGroupPermissionModel
{
    public string Group { get; set; }
    public List<CategoryWisePermissionModel> CategoryWisePermissions { get; set; } = new List<CategoryWisePermissionModel>();
}