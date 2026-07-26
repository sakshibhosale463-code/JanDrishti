using Project.Admin.Models.Catalog;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Users;
using Project.Services.Catalog;
using Project.Services.Helpers;
using Project.Services.Security;
using Project.Services.Security.DataModels;
using Project.Web.Framework.Infrastructure.Mapper.Extensions;
using Project.Web.Framework.Models;
using Project.Web.Framework.Models.Extensions;

namespace Project.Admin.Factories;

public partial record UserRoleModelFactory : IUserRoleModelFactory
{
    #region Fields

    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly IUserRoleService _userRoleService;
    private readonly IPermissionService _permissionService;

    #endregion

    #region Constructor

    public UserRoleModelFactory(IDateTimeHelper dateTimeHelper, 
        IUserRoleService userRoleService,
        IPermissionService permissionService)
    {
        _dateTimeHelper = dateTimeHelper;
        _userRoleService = userRoleService;
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare paged user role list model
    /// </summary>
    /// <param name="searchModel">User role search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role list model
    /// </returns>
    public async Task<UserRoleListModel> PrepareUserRoleListModelAsync(UserRoleSearchModel searchModel)
    {
        if (searchModel is null)
            throw new ArgumentNullException(nameof(searchModel));

        var userRoles = await _userRoleService.GetAllUserRoleAsync(searchModel.SearchName,
            searchModel.Page, searchModel.PageSize, true);

        //prepare grid model
        var listModel = await new UserRoleListModel().PrepareToGridAsync(searchModel, userRoles, () =>
        {
            return userRoles.SelectAwait(async userRole =>
            {
                //fill in model values from the entity
                var model = userRole.ToModel<UserRoleModel>();

                model.CreatedDate = await _dateTimeHelper.ConvertToUserTimeAsync(userRole.CreatedOnUtc);
                model.UpdatedDate = await _dateTimeHelper.ConvertToUserTimeAsync(userRole.UpdatedOnUtc);

                return model;
            });
        });

        return listModel;
    }

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
    public Task<UserRoleModel> PrepareUserRoleModelAsync(UserRoleModel model, UserRole userRole, bool excludeProperties = false)
    {
        if (userRole != null)
        {
            model ??= userRole.ToModel<UserRoleModel>();
        }

        return Task.FromResult(model);
    }

    #region Permissions

    /// <summary>
    /// Prepare user role permission model
    /// </summary>
    /// <param name="userRole">User role</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role permission model
    /// </returns>
   
    public async Task<List<UserRoleGroupPermissionDataModel>> PrepareUserRolePermissionModelAsync(UserRole role)
    {
        if (role == null)
            throw new ArgumentNullException(nameof(role));
        var model = await _userRoleService.GetAllUserRoleGroupPermissionListAsync(role.Id);
        return model;
    }


    #endregion

    #endregion
}

