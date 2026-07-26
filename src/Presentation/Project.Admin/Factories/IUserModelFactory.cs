using Project.Admin.Area.Model;
using Project.Admin.Models.Users;
using Project.Core.Domain.Users;
using Project.Services.Security.DataModels;

namespace Project.Admin.Factories;

/// <summary>
/// Represents user model factory interface
/// </summary>
public interface IUserModelFactory
{
    /// <summary>
    /// Prepare user authentication model
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user auth model
    /// </returns>
    Task<UserAuthModel> PrepareUserAuthModelAsync(User user);

    /// <summary>
    /// Prepare paged user list model
    /// </summary>
    /// <param name="searchModel">User search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user list model
    /// </returns>

    /// <summary>
    /// Prepare user model
    /// </summary>
    /// <param name="model">User model</param>
    /// <param name="user">User</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user model
    /// </returns>
    Task<UserModel> PrepareUserModelAsync(UserModel model, User user, bool excludeProperties = false);

    Task<UserProfileModel> PrepareProfileModelAsync(User user);
    Task<UserListModel> PrepareUserListModelAsync(UserSearchModel searchModel);

    #region Permissions

    #endregion
}
