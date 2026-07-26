using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Services.Common.DataModel;

namespace Project.Services.Security.DataModels
{
    public class CategoryWisePermissionModel
    {
        public string Category { get; set; }
        public List<SelectListDataModel> Permissions { get; set; } = new List<SelectListDataModel>();
    }
    public class UserGroupPermissionDataModel
    {
        public string Group { get; set; }
        public List<CategoryWisePermissionModel> CategoryWisePermissions { get; set; } = new List<CategoryWisePermissionModel>();
    }
}
