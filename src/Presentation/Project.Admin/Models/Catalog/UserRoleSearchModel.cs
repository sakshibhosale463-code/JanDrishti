using Project.Web.Framework.Models;

namespace Project.Admin.Models.Catalog;

public partial record UserRoleSearchModel : BaseSearchModel
{
    #region Properties

    public string SearchName { get; set; }

    #endregion
}
