using Project.Web.Framework.Models;

namespace Project.Admin.Models.Catalog;

public partial record LocationSearchModel : BaseSearchModel
{
    #region Properties

    public string SearchName { get; set; }
    public string SearchCode { get; set; }

    #endregion
}
