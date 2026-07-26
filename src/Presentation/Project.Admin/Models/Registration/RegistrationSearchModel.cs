using Project.Web.Framework.Models;

namespace Project.Admin.Models.Registration;

public partial record RegistrationSearchModel: BaseSearchModel
{
    public string SearchText { get; set; }
    public string OrderBy { get; set; }
}
