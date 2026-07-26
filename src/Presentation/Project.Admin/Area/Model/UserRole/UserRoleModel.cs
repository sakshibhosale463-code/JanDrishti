using Project.Web.Framework.Models;

namespace Project.Admin.Area.Model.UserRole;

public partial record UserRoleModel:BaseModel
{
    #region Properties
    public string Name { get; set; }
    public bool Active { get; set; }
    public string Remark { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    #endregion
}
