using Project.Web.Framework.Models;

namespace Project.Admin.Models.Catalog;

/// <summary>
/// Represent location model
/// </summary>
public partial record LocationModel : BaseModel
{
    #region Properties
    public string Name { get; set; }
    public string Code { get; set; }
    public string Remark { get; set; }
    public bool Active { get; set; }
    public string LogoFileUrl { get; set; }
    public IFormFile LogoFile { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    #endregion
}
