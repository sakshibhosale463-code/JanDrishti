using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Core;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Users;
using Project.Services.Common.DataModel;
using Project.Services.Security.DataModels;
using Project.Services.UserRoles.EntityMode;
using StackExchange.Redis;

namespace Project.Services.Catalog;
public partial interface IUserRoleService
{
    /// <summary>
    /// Gets a UserRole
    /// </summary>
    /// <param name="userRoleId">UserRole identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the UserRole
    /// </returns>
    Task<UserRole> GetUserRoleByIdAsync(long userRoleId);

    /// <summary>
    /// Get user role select list
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the select data list
    /// </returns>
    Task<IList<SelectListDataModel>> GetSelectDataModelListAsync();

    /// <summary>
    /// Check whether user role name already exists or not
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// <param name="roleId">Role identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    Task<bool> HasUserRoleByNameAsync(string roleName, long roleId = 0);

    /// <summary>
    /// Insert UserRole
    /// </summary>
    /// <param name="userRole">userRole</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the userRole
    /// </returns>
    Task InsertUserRoleAsync(UserRole userRole);

    /// <summary>
    /// Update UserRole
    /// </summary>
    /// <param name="userRole">userRole</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the userRole
    /// </returns>
    Task UpdateUserRoleAsync(UserRole userRole);

    /// <summary>
    /// Delete userRole
    /// </summary>
    /// <param name="userRole"></param>
    /// <returns></returns>
    Task DeleteUserRoleAsync(UserRole userRole);

    /// <summary>
    /// Get all paged user role list
    /// </summary>
    /// <param name="userRoleName">User role name</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">Whether to show hidden locations</param>
    /// <returns></returns>
    Task<IPagedList<UserRole>> GetAllUserRoleAsync(string userRoleName = null,
         int pageIndex = 0, int pageSize = int.MaxValue,
         bool showHidden = false);
    Task<IQueryable<UserRole>> GetAllUserRoleAsync(string userRoleName = null);

    Task<IList<PermissionRecordEntityModel>> GetAllPermissionRecordListAsync(long userRoleId);

    /// <summary>
    /// Gets an user role by role name
    /// </summary>
    /// <param name="roleName">User role name</param>
    /// <param name="id">User role identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role
    /// </returns>
    Task<UserRole> GetUserRoleByNameAsync(string roleName, long id = 0);

    

    #region Permissions

    /// <summary>
    /// Get permissions select data model list
    /// </summary>
    /// <param name="userRole">User role</param>
    /// <returns></returns>
    Task<IList<SelectListDataModel>> GetPermissionSelectDataModelListByRoleAsync(UserRole userRole);

    /// <summary>
    /// Get permissions select data model list
    /// </summary>
    /// <param name="userRole">User role</param>
    /// <returns></returns>
    Task<IList<SelectListItem>> GetUserRoleSelectListAsync();
    /// <summary>
    /// Get permissions select data model list
    /// </summary>
    /// <param name="userRole">User role</param>
    /// <returns></returns>
    Task<List<UserRoleGroupPermissionDataModel>> GetAllUserRoleGroupPermissionListAsync(long roleId);

    #endregion
}
