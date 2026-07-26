using Project.Web.Framework.Models;

namespace Project.Admin.Models.Common;

public partial record GlobalSearchModel : BaseNopEntityModel//BaseEntity
{

}

public partial record SearchModelStore : BaseSearchModel
{
    public string SearchText { get; set; }
    public string OrderByColumn { get; set; }

}
