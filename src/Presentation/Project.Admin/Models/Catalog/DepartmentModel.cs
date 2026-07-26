using Project.Web.Framework.Models;

namespace Project.Admin.Models.Catalog;

/// <summary>
/// Represent department model
/// </summary>
public partial record DepartmentModel : BaseModel
{
    #region Properties
    public string Name { get; set; }
    public string Code { get; set; }
    public bool Active { get; set; }
    public string Remark { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    #endregion
}
