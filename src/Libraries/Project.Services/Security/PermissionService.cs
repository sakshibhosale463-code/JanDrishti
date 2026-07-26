using Project.Core;
using Project.Core.Caching;
using Project.Core.Domain.Security;
using Project.Data;
using Project.Services.Common.DataModel;
using Project.Services.Users;

namespace Project.Services.Security;

/// <summary>
/// Permission service
/// </summary>
public partial class PermissionService : IPermissionService
{
    #region Fields

    private readonly IUserService _userService;
    private readonly IWorkContext _workContext;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IRepository<PermissionRecord> _permissionRecordRepository;
    private readonly IRepository<PermissionRecordUserRoleMapping> _permissionRecordUserRoleMappingRepository;

    #endregion

    #region Ctor

    public PermissionService(IUserService userService,
        IWorkContext workContext,
        IStaticCacheManager staticCacheManager,
        IRepository<PermissionRecord> permissionRecordRepository,
        IRepository<PermissionRecordUserRoleMapping> permissionRecordUserRoleMappingRepository)
    {
        _userService = userService;
        _workContext = workContext;
        _staticCacheManager = staticCacheManager;
        _permissionRecordRepository = permissionRecordRepository;
        _permissionRecordUserRoleMappingRepository = permissionRecordUserRoleMappingRepository;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Get permission records by customer role identifier
    /// </summary>
    /// <param name="userRoleId">Customer role identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the permissions
    /// </returns>
    protected virtual async Task<IList<PermissionRecord>> GetPermissionRecordsByUserIdAsync(long userId)
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(ProjectSecurityDefaults.PermissionRecordsAllCacheKey, userId);

        var query = from pr in _permissionRecordRepository.Table
                    join prcrm in _permissionRecordUserRoleMappingRepository.Table on pr.Id equals prcrm
                        .PermissionRecordId
                    where prcrm.UserRoleId == userId
                    orderby pr.Id
                    select pr;

        return await _staticCacheManager.GetAsync(key, async () => await query.ToListAsync());
    }

    /// <summary>
    /// Delete a permission
    /// </summary>
    /// <param name="permission">Permission</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task DeletePermissionRecordAsync(PermissionRecord permission)
    {
        await _permissionRecordRepository.DeleteAsync(permission);
    }

    /// <summary>
    /// Gets a permission
    /// </summary>
    /// <param name="systemName">Permission system name</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the permission
    /// </returns>
    protected virtual async Task<PermissionRecord> GetPermissionRecordBySystemNameAsync(string systemName)
    {
        if (string.IsNullOrWhiteSpace(systemName))
            return null;

        var query = from pr in _permissionRecordRepository.Table
                    where pr.SystemName == systemName
                    orderby pr.Id
                    select pr;

        var permissionRecord = await query.FirstOrDefaultAsync();
        return permissionRecord;
    }

    /// <summary>
    /// Inserts a permission
    /// </summary>
    /// <param name="permission">Permission</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task InsertPermissionRecordAsync(PermissionRecord permission)
    {
        await _permissionRecordRepository.InsertAsync(permission);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all permissions
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the permissions
    /// </returns>
    public virtual async Task<IList<PermissionRecord>> GetAllPermissionRecordsAsync()
    {
        var permissions = await _permissionRecordRepository.GetAllAsync(query =>
        {
            return from pr in query
                   orderby pr.Name
                   select pr;
        });

        return permissions;
    }

    /// <summary>
    /// Get permissions select data model list
    /// </summary>
    /// <returns></returns>
    public async Task<IList<SelectListDataModel>> GetAllPermissionRecordsSelectDataModelListAsync()
    {
        var query = from permission in _permissionRecordRepository.Table
                    select new SelectListDataModel
                    {
                        Id = permission.Id,
                        Name = $"{permission.Category}-{permission.Name}"
                    };

        var permissions = await query.ToListAsync();

        return permissions;
    }
    public async Task<List<string>> GetUserPermissionListAsync(long userId)
    {
        var query = from permission in _permissionRecordRepository.Table
                    join userPermission in _permissionRecordUserRoleMappingRepository.Table
                    on permission.Id equals userPermission.PermissionRecordId
                    where userPermission.UserRoleId == userId
                    select permission.SystemName;

        return await query.ToListAsync();
    }

    /// <summary>
    /// Updates the permission
    /// </summary>
    /// <param name="permission">Permission</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdatePermissionRecordAsync(PermissionRecord permission)
    {
        await _permissionRecordRepository.UpdateAsync(permission);
    }

    /// <summary>
    /// Authorize permission
    /// </summary>
    /// <param name="permission">Permission record</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the rue - authorized; otherwise, false
    /// </returns>
    public virtual async Task<bool> AuthorizeAsync(PermissionRecord permission)
    {
        return await AuthorizeAsync(permission, await _workContext.GetCurrentUserAsync());
    }

    /// <summary>
    /// Authorize permission
    /// </summary>
    /// <param name="permission">Permission record</param>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the rue - authorized; otherwise, false
    /// </returns>
    public virtual async Task<bool> AuthorizeAsync(PermissionRecord permission, Core.Domain.Users.User user)
    {
        if (permission == null)
            return false;

        if (user == null)
            return false;

        return await AuthorizeAsync(permission.SystemName, user);
    }

    /// <summary>
    /// Authorize permission
    /// </summary>
    /// <param name="permissionRecordSystemName">Permission record system name</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the rue - authorized; otherwise, false
    /// </returns>
    public virtual async Task<bool> AuthorizeAsync(string permissionRecordSystemName)
    {
        return await AuthorizeAsync(permissionRecordSystemName, await _workContext.GetCurrentUserAsync());
    }

    /// <summary>
    /// Authorize permission
    /// </summary>
    /// <param name="permissionRecordSystemName">Permission record system name</param>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the rue - authorized; otherwise, false
    /// </returns>
    public virtual async Task<bool> AuthorizeAsync(string permissionRecordSystemName, Core.Domain.Users.User user)
    {
        if (string.IsNullOrEmpty(permissionRecordSystemName))
            return false;

        //  var role = await _userService.GetUserRoleAsync(user.Id);
        if (await AuthorizeAsync(permissionRecordSystemName, user.RoleId))
            //yes, we have such permission
            return true;

        //no permission found
        return false;
    }

    /// <summary>
    /// Authorize permission
    /// </summary>
    /// <param name="permissionRecordSystemName">Permission record system name</param>
    /// <param name="userRoleId">User role identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the rue - authorized; otherwise, false
    /// </returns>
    public virtual async Task<bool> AuthorizeAsync(string permissionRecordSystemName, long userId)
    {
        if (string.IsNullOrEmpty(permissionRecordSystemName))
            return false;

        var key = _staticCacheManager.PrepareKeyForDefaultCache(ProjectSecurityDefaults.PermissionAllowedCacheKey, permissionRecordSystemName, userId);

        return await _staticCacheManager.GetAsync(key, async () =>
        {
            var permissions = await GetPermissionRecordsByUserIdAsync(userId);
            foreach (var permission in permissions)
                if (permission.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        });
    }

    /// <summary>
    /// Gets a permission record-user role mapping
    /// </summary>
    /// <param name="permissionId">Permission identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<IList<PermissionRecordUserRoleMapping>> GetMappingByPermissionRecordIdAsync(long permissionId)
    {
        var query = _permissionRecordUserRoleMappingRepository.Table;

        query = query.Where(x => x.PermissionRecordId == permissionId);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Delete a permission record-customer role mapping
    /// </summary>
    /// <param name="permissionId">Permission identifier</param>
    /// <param name="userRoleId">Customer role identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeletePermissionRecordCustomerRoleMappingAsync(long permissionId, long userRoleId)
    {
        var mapping = _permissionRecordUserRoleMappingRepository.Table
            .FirstOrDefault(prcm => prcm.UserRoleId == userRoleId && prcm.PermissionRecordId == permissionId);
        if (mapping is null)
            return;

        await _permissionRecordUserRoleMappingRepository.DeleteAsync(mapping);
    }

    /// <summary>
    /// Inserts a permission record-user role mapping
    /// </summary>
    /// <param name="permissionRecordUserRoleMapping">Permission record-user role mapping</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertPermissionRecordCustomerRoleMappingAsync(PermissionRecordUserRoleMapping permissionRecordUserRoleMapping)
    {
        await _permissionRecordUserRoleMappingRepository.InsertAsync(permissionRecordUserRoleMapping);
    }

    /// <summary>
    /// Get existing user permission role mapping data 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="Ids"></param>
    /// <returns></returns>
    public async Task<IList<PermissionRecordUserRoleMapping>> GetAllRemovePermissionRecordUserRoleMappingByRoleIdAsync(long roleId)
    {
        if (roleId == 0)
            throw new ArgumentException(nameof(roleId));

        var query = from uc in _permissionRecordUserRoleMappingRepository.Table
                    where uc.UserRoleId == roleId
                    select uc;

        var list = await query.ToListAsync();
        return list;
    }

    /// <summary>
    /// Delete a permission record-customer role mapping
    /// </summary>
    /// <param name="permissionId">Permission identifier</param>
    /// <param name="customerRoleId">Customer role identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeletePermissionRecordUserRoleMappingAsync(long permissionId, long customerRoleId)
    {
        var mapping = _permissionRecordUserRoleMappingRepository.Table
            .FirstOrDefault(prcm => prcm.UserRoleId == customerRoleId && prcm.PermissionRecordId == permissionId);
        if (mapping is null)
            return;

        await _permissionRecordUserRoleMappingRepository.DeleteAsync(mapping);
    }

    /// <summary>
    /// Inserts a permission record-customer role mapping
    /// </summary>
    /// <param name="permissionRecordCustomerRoleMapping">Permission record-customer role mapping</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertPermissionRecordUserRoleMappingAsync(PermissionRecordUserRoleMapping permissionRecordUserRoleMapping)
    {
        await _permissionRecordUserRoleMappingRepository.InsertAsync(permissionRecordUserRoleMapping);
    }
    #endregion
}