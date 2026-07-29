using Project.Admin.Area.Model;
using Project.Admin.Models.Users;
using Project.Core;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Users;
using Project.Core.Security;
using Project.Services.Authentication;
using Project.Services.Catalog;
using Project.Services.Helpers;
using Project.Services.Registration;
using Project.Services.Security;
using Project.Services.Users;
using Project.Web.Framework.Infrastructure.Mapper.Extensions;
using Project.Web.Framework.Models;
using Project.Web.Framework.Models.Extensions;
using Project.Web.Framework.Security;

namespace Project.Admin.Factories;

/// <summary>
/// Represents user model factory
/// </summary>
public partial record UserModelFactory : IUserModelFactory
{
    #region Fields
    private readonly IWorkContext _workContext;
    private readonly IUserService _userService;
    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly IDepartmentService _departmentService;
    private readonly IPermissionService _permissionService;
    private readonly IUserRoleService _userRoleService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IRegistrationMasterService _registrationMasterService;

    #endregion

    #region Constructor

    public UserModelFactory(IUserService userService, IWorkContext workContext,
        IDateTimeHelper dateTimeHelper,
        IDepartmentService departmentService,
        IPermissionService permissionService,
        IUserRoleService userRoleService,
        IAuthenticationService authenticationService, IRegistrationMasterService registrationMasterService)
    {
        _workContext = workContext;
        _userService = userService;
        _dateTimeHelper = dateTimeHelper;
        _departmentService = departmentService;
        _permissionService = permissionService;
        _userRoleService = userRoleService;
        _authenticationService = authenticationService;
        _registrationMasterService = registrationMasterService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare user authentication model
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user auth model
    /// </returns>
    public async Task<UserAuthModel> PrepareUserAuthModelAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var authModel = user.ToModel<UserAuthModel>();
        authModel.Token = await _authenticationService.GenerateTokenAsync(user);
        authModel.RoleName = (await _userRoleService.GetUserRoleByIdAsync(user.RoleId)).Name;
        authModel.Permissions = await _permissionService.GetUserPermissionListAsync(user.RoleId);
        return authModel;
    }



    /// <summary>
    /// Prepare user model
    /// </summary>
    /// <param name="model">User model</param>
    /// <param name="user">User</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the genre model
    /// </returns>
    public async Task<UserModel> PrepareUserModelAsync(UserModel model, User user, bool excludeProperties = false)
    {

        // show Trainer only assigne domain
        var currentUser = await _workContext.GetCurrentUserAsync();
        List<long> domainIds = null;

        if (user != null)
        {
            model ??= user.ToModel<UserModel>();
            model.Password = EncryptionHelper.DecryptPassword(user.Password);

            // Role
            var roledata = await _userRoleService.GetUserRoleByIdAsync(user.RoleId);
            if (roledata != null)
                model.Role = roledata.Name;

            if (currentUser != null)
            {
                var userRole = await _userRoleService.GetUserRoleByIdAsync(currentUser.RoleId);
               
            }
        }

        return model;
    }

    public async Task<UserProfileModel> PrepareProfileModelAsync(User user)
    {
        // Fetch user profile and map to model
        var model = user.ToModel<UserProfileModel>();
        model.Password = EncryptionHelper.DecryptPassword(user.Password);

        // Role
        var roledata = await _userRoleService.GetUserRoleByIdAsync(user.RoleId);
        if (roledata != null)
        {
            model.Role = roledata.Name;
            model.SystemName = roledata.SystemName;
        }

        var currentUser = await _workContext.GetCurrentUserAsync();
        
        if (currentUser != null)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(currentUser.RoleId);
            
        }

        return model;
    }

    public async Task<UserListModel> PrepareUserListModelAsync(UserSearchModel searchModel)
    {
        if (searchModel is null)
            throw new ArgumentNullException(nameof(searchModel));

        var userregistration = await _userService.GetAllUsersAsync(searchText: searchModel.SearchText,
               pageIndex: searchModel.Page, pageSize: searchModel.PageSize);

        //prepare grid model
        var model = await new UserListModel().PrepareToGridAsync(searchModel, userregistration, () =>
        {
            return userregistration.SelectAwait(async user =>
            {
                var model = user.ToModel<UserModel>();

                var role = await _userRoleService.GetUserRoleByIdAsync(user.RoleId);
                model.Role = role.Name ?? string.Empty;
               
                return model;
            });
        });
        return model;
    }

    #region Permissions


    #endregion

    #endregion
}
