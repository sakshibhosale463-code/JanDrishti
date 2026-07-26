using System.Data;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Core;
using Project.Core.Caching;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Security;
using Project.Core.Domain.Users;
using Project.Data;
using Project.Services.Common.DataModel;
using Project.Services.Directory;
using Project.Services.Security.DataModels;
using Project.Services.UserRoles.EntityMode;

namespace Project.Services.Catalog;

public class UserRoleService : IUserRoleService
{
    #region Fields

    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IStaticCacheManager _staticCacheManager;
    //private readonly IUserService _userService;
    private readonly IRepository<PermissionRecord> _permissionRepository;
    private readonly IRepository<PermissionRecordEntityModel> _permissionRecordEntityModel;
    private readonly IRepository<PermissionRecordUserRoleMapping> _permissionRecordUserRoleMappingRepository;
    
    #endregion

    #region Constructor

    public UserRoleService(IRepository<UserRole> userRoleRepository,
        IRepository<PermissionRecord> permissionRepository,
        IStaticCacheManager staticCacheManager,
        //IUserService userService,
        IRepository<PermissionRecordUserRoleMapping> permissionRecordUserRoleMappingRepository,
        IRepository<PermissionRecordEntityModel> permissionRecordEntityModel
        )
    {

        _userRoleRepository = userRoleRepository;
        _permissionRepository = permissionRepository;
        _staticCacheManager = staticCacheManager;
        //_userService = userService;
        _permissionRecordEntityModel = permissionRecordEntityModel;
        _permissionRecordUserRoleMappingRepository = permissionRecordUserRoleMappingRepository;
       
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a user role
    /// </summary>
    /// <param name="userRoleId">userRole identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the UserRole
    /// </returns>
    public async Task<UserRole> GetUserRoleByIdAsync(long userRoleId)
    {
        return await _userRoleRepository.GetByIdAsync(userRoleId);
    }

    /// <summary>
    /// Get user role select list
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the select data list
    /// </returns>
    public async Task<IList<SelectListDataModel>> GetSelectDataModelListAsync()
    {
        var query = from d in _userRoleRepository.Table
                    where d.Active && d.Deleted == false && d.SystemName!=ProjectUserDefaults.StudentRoleName
                    select new SelectListDataModel
                    {
                        Id = d.Id,
                        Name = d.Name
                    };

        return await query.ToListAsync();
    }

    /// <summary>
    /// Check whether user role name already exists or not
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// <param name="roleId">Role identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public async Task<bool> HasUserRoleByNameAsync(string roleName, long roleId = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        var query = _userRoleRepository.Table;
        if (roleId != 0)
            query = query.Where(ur => ur.Id != roleId);

        var isExist = await query.AnyAsync(ur => ur.Name == roleName && ur.Deleted == false);

        return isExist;
    }

    /// <summary>
    /// Insert user role
    /// </summary>
    /// <param name="userRole">userRole</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the userRole
    /// </returns>
    public async Task InsertUserRoleAsync(UserRole userRole)
    {
        await _userRoleRepository.InsertAsync(userRole);
    }

    /// <summary>
    /// Update user role
    /// </summary>
    /// <param name="userRole">userRole</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the userRole
    /// </returns>
    public async Task UpdateUserRoleAsync(UserRole userRole)
    {
        await _userRoleRepository.UpdateAsync(userRole);
    }

    /// <summary>
    /// Delete user role
    /// </summary>
    /// <param name="userRole">userRole</param>
    /// <returns></returns>
    public async Task DeleteUserRoleAsync(UserRole userRole)
    {
        await _userRoleRepository.DeleteAsync(userRole);
    }

    /// <summary>
    /// Get all paged user role list
    /// </summary>
    /// <param name="userRoleName">User role name</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">Whether to show hidden locations</param>
    /// <returns></returns>
    public async Task<IPagedList<UserRole>> GetAllUserRoleAsync(string userRoleName = null,
         int pageIndex = 0, int pageSize = int.MaxValue,
         bool showHidden = false)
    {
        var result = await _userRoleRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(userRoleName))
                query = query.Where(u => u.Name.Contains(userRoleName));

            if (!showHidden)
                query = query.Where(u => u.Active);
            query = query.Where(u => !u.Deleted);

            query = query.OrderByDescending(u => u.Id);

            return query;
        }, pageIndex, pageSize);

        return result;
    }
    public async Task<IQueryable<UserRole>> GetAllUserRoleAsync(string userRoleName = null)
    {
        var query = from r in _userRoleRepository.Table
                    where r.Deleted == false && r.Active == true && (string.IsNullOrEmpty(userRoleName) || r.Name.Contains(userRoleName))
                    select r;

        return await Task.FromResult(query);
    }

    #region Permissions

    /// <summary>
    /// Get permissions select data model list
    /// </summary>
    /// <param name="userRole">User role</param>
    /// <returns></returns>
    public async Task<IList<SelectListDataModel>> GetPermissionSelectDataModelListByRoleAsync(UserRole userRole)
    {
        if (userRole == null)
            throw new ArgumentNullException(nameof(userRole));

        var query = from permissionUserRole in _permissionRecordUserRoleMappingRepository.Table
                    join permission in _permissionRepository.Table on permissionUserRole.PermissionRecordId equals permission.Id
                    where permissionUserRole.UserRoleId == userRole.Id
                    select new SelectListDataModel
                    {
                        Id = permission.Id,
                        Name = $"{permission.Category}-{permission.Name}"
                    };

        var permissions = await query.ToListAsync();

        return permissions;
    }

    /// <summary>
    /// Get user role select data model list
    /// </summary>
    /// <param name="userRole">User role</param>
    /// <returns></returns>

    public async Task<IList<SelectListItem>> GetUserRoleSelectListAsync()
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(DirectoryDefault.UserRoleSelectDataAllCacheKey);
        var list = await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var query = _userRoleRepository.Table;
            query = query.Where(c => !c.Deleted);
            var selectListItems = query.OrderBy(x => x.Name).Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            return await selectListItems.ToListAsync();
        });
        return list;
    }


    public async Task<List<UserRoleGroupPermissionDataModel>> GetAllUserRoleGroupPermissionListAsync(long roleId)
    {
        var query = from permission in _permissionRepository.Table
                    join userRolePermission in _permissionRecordUserRoleMappingRepository.Table
                    on new { PermissionId = permission.Id, RoleId = roleId }
                    equals new { PermissionId = userRolePermission.PermissionRecordId, RoleId = userRolePermission.UserRoleId }
                    into userRolePermissions // Perform LEFT JOIN
                    from userPermission in userRolePermissions.DefaultIfEmpty() // Handle NULL values for unmatched records
                    select new
                    {
                        Category = permission.Category,
                        Name = permission.Name,
                        Id = permission.Id,
                        IsChecked = userPermission != null // Set to true if user has permission, otherwise false
                    };

        var mainQuery = await query.ToListAsync().ConfigureAwait(false);

        var data = mainQuery
            .GroupBy(p => p.Category)
            .Select(g => new UserRoleGroupPermissionDataModel
            {
                Group = g.Key,
                CategoryWisePermissions = g.GroupBy(p => p.Category)
                    .Select(c => new CategoryWisePermissionModel
                    {
                        Category = c.Key,
                        Permissions = c.Select(p => new SelectListDataModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Code = p.Name,
                            IsChecked = p.IsChecked // Assign IsChecked based on user permission check
                        }).ToList()
                    }).ToList()
            }).ToList();

        return data;
    }

    /// <summary>
    /// Gets an permission list of data
    /// </summary>
    /// <returns></returns>
    public async Task<IList<PermissionRecordEntityModel>> GetAllPermissionRecordListAsync(long userRoleId)
    {
        var parameterList = new List<DataParameter>
        {
            new DataParameter("@UserRoleId", userRoleId)
        };
        var userRolePermissionList = await _permissionRecordEntityModel.EntityFromSqlAsync("dbo.spGetUserRolePermissionData", parameterList.ToArray());

        // Use the PagedList ctor that accepts totalRecords
        return userRolePermissionList;
    }

    /// <summary>
    /// Gets an user role by role name
    /// </summary>
    /// <param name="roleName">User role name</param>
    /// <param name="id">User role identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role
    /// </returns>
    public async Task<UserRole> GetUserRoleByNameAsync(string roleName, long id = 0)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentNullException(nameof(roleName));

        var query = _userRoleRepository.Table;
        if (id != 0)
        {
            query = query.Where(role => role.Id != id && role.Deleted == true);
        }

        var userRole = await query.FirstOrDefaultAsync(r => r.SystemName.Equals(roleName));
        return userRole;
    }


    #endregion

    #endregion
}
