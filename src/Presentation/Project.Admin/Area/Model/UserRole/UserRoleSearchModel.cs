using Project.Web.Framework.Models;

namespace Project.Admin.Area.Model.UserRole;

public partial record UserRoleSearchModel : BaseSearchModel
{
    #region Properties

    public string SearchName { get; set; }

    #endregion
}
