using Microsoft.AspNetCore.Mvc;
using Project.Admin.Factories;
using Project.Admin.Models.Catalog;
using Project.Admin.Models.Users;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Security;
using Project.Services.Catalog;
using Project.Services.Security;
using Project.Web.Framework;
using Project.Web.Framework.Infrastructure.Mapper.Extensions;

namespace Project.Admin.Area.Admin;
public class UserRoleController : BaseProtectedController
{
    #region Fields

    private readonly IUserRoleService _userRoleService;
    private readonly IPermissionService _permissionService;
    private readonly IUserRoleModelFactory _userRoleModelFactory;

    #endregion

    #region Constructor

    public UserRoleController(IUserRoleService userRoleService,
        IPermissionService permissionService,
        IUserRoleModelFactory userRoleModelFactory)
    {
        _userRoleService = userRoleService;
        _permissionService = permissionService;
        _userRoleModelFactory = userRoleModelFactory;
    }

    #endregion


    #region Methods

    [HttpGet]
    public async Task<IActionResult> GetUserRoleSelectList()
    {
        var userRole = await _userRoleService.GetUserRoleSelectListAsync();
        return Success(userRole);
    }


    //-----------------------------------------------------------------

    [HttpPost]
    public async Task<IActionResult> CreateUserRole(UserRoleModel model)
    {
        var existingUserRole = await _userRoleService.GetUserRoleByNameAsync(model.Name);
        if (existingUserRole != null)
            return Success("User role alrady exist.");

        var userRole = model.ToEntity<UserRole>();
        userRole.SystemName = model.Name;
        userRole.CreatedOnUtc = DateTime.UtcNow;
        userRole.UpdatedOnUtc = DateTime.UtcNow;
        await _userRoleService.InsertUserRoleAsync(userRole);

        return Success(userRole.Id, "User role added successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetUserRoleList([FromQuery] UserRoleSearchModel searchModel)
    {
        var roleList = await _userRoleModelFactory.PrepareUserRoleListModelAsync(searchModel);
        if (roleList == null)
            return Error("", "Data not found.");

        return PagedList(roleList);
    }



    [HttpGet]
    public async Task<IActionResult> GetUserRoleDetails(long id)
    {
        var userRole = await _userRoleService.GetUserRoleByIdAsync(id);
        if (userRole == null)
            return Error("", "User not found.");

        //prepare model
        var model = await _userRoleModelFactory.PrepareUserRoleModelAsync(null, userRole);

        return Success(model, "Success");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserRole(UserRoleModel model, bool continueEditing)
    {
        var userRole = await _userRoleService.GetUserRoleByIdAsync(model.Id);
        if (userRole == null)
            return Error("", "User role not found.");

        var existingUserRole = await _userRoleService.GetUserRoleByNameAsync(model.Name, userRole.Id);
        if (existingUserRole != null)
            return Error("", "User role already exist.");

        if (ModelState.IsValid)
        {
            userRole = model.ToEntity(userRole);
            userRole.UpdatedOnUtc = DateTime.UtcNow;
            await _userRoleService.UpdateUserRoleAsync(userRole);
        }
        return Success(userRole.Id, "User role updated successfully.");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUserRole(UserRoleModel model)
    {
        var userRoleData = await _userRoleService.GetUserRoleByIdAsync(model.Id);
        if (userRoleData == null)
            return Error("", "User role not found");

        await _userRoleService.DeleteUserRoleAsync(userRoleData);

        return Success("", "Success");
    }

    #endregion

    #region UserRole Permission Model

    [HttpGet]
    public async Task<IActionResult> GetUserRolePermissionList([FromQuery] UserSearchModel searchModel)
    {
        var permissionList = await _userRoleService.GetAllPermissionRecordListAsync(searchModel.UserRoleId);

        // var permissionList = await _userRoleModelFactory.PrepareUserRolePermissionListModelAsync(searchModel);
        if (permissionList == null)
            return Error("", "Permissons not found.");

        return Success(permissionList);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserRolePermissionMapping(UserRolePermissionModel model)
    {
        // check exist permissions list against userrole 
        var existpermissionList = await _permissionService.GetAllRemovePermissionRecordUserRoleMappingByRoleIdAsync(model.UserRoleId);

        foreach (var item in existpermissionList)
            // check ui side permissionIds == to db side permissionIds and which is not match that entry deleted
            if (!model.PermissionIds.Contains(item.PermissionRecordId))
                await _permissionService.DeletePermissionRecordUserRoleMappingAsync(item.PermissionRecordId, item.UserRoleId);

        foreach (var item in model.PermissionIds)
        {
            var exists = existpermissionList.Any(x => x.PermissionRecordId == item);
            //var isExist = .Where(x => x.PermissionRecordId == item).FirstOrDefault();
            if (!exists)
            {
                var recordUserRoleMapping = new PermissionRecordUserRoleMapping();
                recordUserRoleMapping.UserRoleId = model.UserRoleId;
                recordUserRoleMapping.PermissionRecordId = item;
                await _permissionService.InsertPermissionRecordUserRoleMappingAsync(recordUserRoleMapping);
            }
        }

        return Success("", "Permission added successfully.");
    }

    #endregion
}
