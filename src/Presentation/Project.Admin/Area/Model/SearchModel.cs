using Project.Web.Framework.Models;

namespace Project.Admin.Area.Model;

public partial record SearchModel: BaseSearchModel
{
    public long LoginUserId { get; set; }
    public long RoleId { get; set; }
    public string SearchText { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
