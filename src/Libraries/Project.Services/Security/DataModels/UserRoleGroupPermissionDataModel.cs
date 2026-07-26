using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Security.DataModels
{
    public class UserRoleGroupPermissionDataModel
    {
        public string Group { get; set; }
        public List<CategoryWisePermissionModel> CategoryWisePermissions { get; set; } = new List<CategoryWisePermissionModel>();
    }
}
