using Project.Admin.Area.Model;
using Project.Admin.Models.Registration;
using Project.Admin.Models.Users;
using Project.Core;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Users;
using Project.Core.Security;
using Project.Services.Authentication;
using Project.Services.Catalog;
using Project.Services.Registration;
using Project.Services.Security;
using Project.Services.Users;
using Project.Web.Framework.Infrastructure.Mapper.Extensions;
using Project.Web.Framework.Models.Extensions;

namespace Project.Admin.Factories;

public class RegistrationModelFactory : IRegistrationModelFactory
{
    #region

    private readonly IWorkContext _workContext;
    private readonly IUserService _userService;
    private readonly IRegistrationMasterService _registrationMasterService;
    private readonly IPermissionService _permissionService;
    private readonly IUserRoleService _userRoleService;
    private readonly IAuthenticationService _authenticationService;

    #endregion

    #region

    public RegistrationModelFactory(IRegistrationMasterService registrationMasterService,
        IPermissionService permissionService,
        IUserRoleService userRoleService,
        IWorkContext workContext,
        IUserService userService,
        IAuthenticationService authenticationService)
    {
        _workContext = workContext;
        _userService = userService;
        _registrationMasterService = registrationMasterService;
        _permissionService = permissionService;
        _userRoleService = userRoleService;
        _authenticationService = authenticationService;
    }

    #endregion

    #region Methods

    public async Task<UserAuthModel> PrepareStudentAuthModelAsync(RegistrationMaster student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        var authModel = student.ToModel<UserAuthModel>();
        authModel.Name = student.FullName;
        authModel.Token = await _authenticationService.GenerateTokenForStudentAsync(student);
        authModel.RoleName = (await _userRoleService.GetUserRoleByIdAsync(student.RoleId)).Name;
        authModel.Permissions = await _permissionService.GetUserPermissionListAsync(student.RoleId);
        return authModel;
    }

    //public async Task<RegistrationListModel> PrepareProjectGyaanRegistrationListModelAsync(RegistrationSearchModel searchModel, bool? onlyPaidUsers = null/*, long? batchId = null*/)
    //{
    //    if (searchModel is null)
    //        throw new ArgumentNullException(nameof(searchModel));

    //    // show Trainer only assigne domain sessions
    //    var currentUser = await _workContext.GetCurrentUserAsync();
    //    IList<long> domainIds = null;
    //    if (currentUser != null)
    //    {
    //        var userRole = await _userRoleService.GetUserRoleByIdAsync(currentUser.RoleId);
    //        if (userRole.SystemName == ProjectUserDefaults.TrainerRoleName)
    //        {
    //            var userDomain = await _userService.GetUserDomainMappingListByUserIdAsync(currentUser.Id);
    //            domainIds = userDomain.Select(x => x.DomainId).ToList();
    //        }
    //    }

    //    var data = await _registrationMasterService.GetAllRegistrationUsersListAsync(searchText: searchModel.SearchText, domainIds,
    //           pageIndex: searchModel.Page, pageSize: searchModel.PageSize, onlyPaidUsers: onlyPaidUsers/*, batchId: batchId*/);

    //    var listmodel = await new RegistrationListModel().PrepareToGridAsync(searchModel, data, () =>
    //    {
    //        return data.SelectAwait(async query =>
    //        {
    //            var model = query.ToModel<UserRegistrationModel>();

    //            var status = await _registrationMasterService.GetCandidateTypeMastersByIdAsync(query.CurrentStatus);

    //            model.CurrentStatusName = status?.name ?? string.Empty;
    //            model.AssessmentStatus = query.AssessmentStatus;
               
    //            var enrollmentData = await _attendanceService.GetEnrollmentByRegisterIdAsync(query.Id);
    //            var type = await _registrationMasterService.GetDomainMasterByIdAsync(enrollmentData.DomainId);
    //            model.Cource = type?.Name ?? string.Empty;

    //            model.PaymentStatus = query.PaymentStatus;
    //            model.IsPayment = query.IsPayment;
    //            model.IsActive = query.IsActive;
    //            model.PaidAmount = query.PaidAmount;
    //            return model;
    //        });
    //    });
    //    return listmodel;
    //}

    /// <summary>
    /// Prepare User model
    /// </summary>
    public async Task<UserRegistrationModel> PrepareProjectGyaanRegistrationModelAsync(UserRegistrationModel model,
        RegistrationMaster registrationMaster, bool excludeProperties = false)
    {
        if (model != null)
        {
            model ??= registrationMaster.ToModel<UserRegistrationModel>();
        }
        // Fetch user profile and map to model
        model = registrationMaster.ToModel<UserRegistrationModel>();
        model.Password = EncryptionHelper.DecryptPassword(registrationMaster.Password);

        // Role
        var roledata=await _userRoleService.GetUserRoleByIdAsync(registrationMaster.RoleId);
        if(roledata!=null)
        {
            model.RoleId= registrationMaster.RoleId;
            model.Role = roledata.Name;
            model.SystemName= roledata.SystemName;
        }
        return model;
    }


    #endregion
}
