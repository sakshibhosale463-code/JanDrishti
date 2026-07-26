using Project.Admin.Models.Catalog;
using Project.Core.Domain.Catalog;
using Project.Services.Security.DataModels;

namespace Project.Admin.Factories;

public interface IUserRoleModelFactory
{
    /// <summary>
    /// Prepare paged user role list model
    /// </summary>
    /// <param name="searchModel">User role search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role list model
    /// </returns>
    Task<UserRoleListModel> PrepareUserRoleListModelAsync(UserRoleSearchModel searchModel);

    /// <summary>
    /// Prepare user role model
    /// </summary>
    /// <param name="model">User role model</param>
    /// <param name="userRole">User role</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role model
    /// </returns>
    Task<UserRoleModel> PrepareUserRoleModelAsync(UserRoleModel model, UserRole userRole, bool excludeProperties = false);

    #region Permissions

    /// <summary>
    /// Prepare user role permission model
    /// </summary>
    /// <param name="userRole">User role</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role permission model
    /// </returns>
    Task<List<UserRoleGroupPermissionDataModel>> PrepareUserRolePermissionModelAsync(UserRole role);

    #endregion
}

