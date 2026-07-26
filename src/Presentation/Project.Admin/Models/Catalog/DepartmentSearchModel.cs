using Project.Web.Framework.Models;

namespace Project.Admin.Models.Catalog;

/// <summary>
/// Represents department search model
/// </summary>
public partial record DepartmentSearchModel : BaseSearchModel
{
    #region Properties

    public string SearchName { get; set; }
    public string SearchCode { get; set; }

    #endregion
}
