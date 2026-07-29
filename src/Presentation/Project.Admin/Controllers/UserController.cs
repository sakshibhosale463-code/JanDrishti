using System.Runtime.ConstrainedExecution;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Admin.Area.Model;
using Project.Admin.Factories;
using Project.Admin.Models.Users;
using Project.Core;
using Project.Core.Domain.Users;
using Project.Core.Security;
using Project.Services.Catalog;
using Project.Services.ExportImport;
using Project.Services.Registration;
using Project.Services.Users;
using Project.Web.Framework;
using Project.Web.Framework.Infrastructure.Mapper.Extensions;

namespace Project.Admin.Controllers;

public class UserController : BaseProtectedController
{
    #region Fields

    private readonly IWorkContext _workContext;
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IUserModelFactory _userModelFactory;
    private readonly IExportManager _exportManager;
    private readonly IRegistrationMasterService _registrationMasterService;
    private readonly IRegistrationModelFactory _projectGyaanRegistrationModelFactory;
    private readonly IConfiguration _configuration;
    #endregion

    #region Constructor

    public UserController(IUserService userService, IWorkContext workContext,
        IUserRoleService userRoleService,
        IUserModelFactory userModelFactory,
        IExportManager exportManager,
        IRegistrationMasterService registrationMasterService,
        IRegistrationModelFactory projectGyaanRegistrationModelFactory, IConfiguration configuration)
    {
        _userService = userService;
        _workContext = workContext;
        _userRoleService = userRoleService;
        _userModelFactory = userModelFactory;
        _exportManager = exportManager;
        _registrationMasterService = registrationMasterService;
        _projectGyaanRegistrationModelFactory = projectGyaanRegistrationModelFactory;
        _configuration = configuration;
    }

    #endregion

    #region Utilities


        #endregion

    #region Methods

        [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetRoleandDomainSelectList()
    {
        //prepare user model
        var model = await _userModelFactory.PrepareUserModelAsync(new UserModel(), null);
        return Success(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetRoleSelectList()
    {
        //prepare user model
        var model = await _userModelFactory.PrepareUserModelAsync(new UserModel(), null);
        return Success(model);
    }

    private async Task<bool> IsEmailOrMobileExists(string email, string mobile)
    {
        // Check in Users
        var user = await _userService.GetUserByEmailAsync(email, mobile);
        if (user != null)
            return true;

        // Check in Students
        var student = await _registrationMasterService.CheckCandidateExistAsync(email, mobile);
        if (student != null)
            return true;

        return false;
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] UserModel model)
    {
        if (model == null)
            return Error(model, "Model is Invalid.");

        var exists = await IsEmailOrMobileExists(model.EmailAddress, model.MobileNumber);
        if (exists)
            return Error("", "Email or mobile number already exists.");

        //Get Role
        var userRole = await _userRoleService.GetUserRoleByIdAsync(model.RoleId);
        if (userRole == null || userRole.Deleted)
            return Error(string.Empty, "User role not found.");

        var user = model.ToEntity<User>();
        user.CreatedOnUtc = DateTime.Now;
        user.UpdatedOnUtc = DateTime.Now;
        user.Active = true;
        user.Password = EncryptionHelper.EncryptPassword(user.Password);
        await _userService.InsertUserAsync(user);

        model.Id = user.Id;

        return Success(user, "User inserted successfully.");
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UserModel model)
    {
        var user = await _userService.GetUserByIdAsync(model.Id);
        if (user == null || user.Deleted)
            return Error("User not found.");

        var exists = await _registrationMasterService.IsEmailOrMobileExistsForUpdate( model.EmailAddress, model.MobileNumber, userId: model.Id, studentId: null);
        if (exists)
            return Error("", "Email or mobile number already exists.");

        user = model.ToEntity(user);
        user.UpdatedOnUtc = DateTime.Now;
        user.Password = EncryptionHelper.EncryptPassword(model.Password);
        await _userService.UpdateUserAsync(user);

        model.Id = user.Id;

        return Success(user, "User Updated successfully.");
    }


    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] UserModel model)
    {
        var user = await _userService.GetUserByIdAsync(model.Id);
        if (user == null || user.Deleted)
            return Error("User not found.");

        //delete user
        await _userService.DeleteUserAsync(user);
        return Success(model.Id, "User deleted successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var currentUser = await _workContext.GetCurrentUserAsync();
        var currentStudent = await _workContext.GetCurrentStudentsync();

        // Show profile data as per role
        if (currentUser != null)
        {
            var user = await _userService.GetUserByIdAsync(currentUser.Id);
            if (user == null)
                return Error("", "User not found");

            var model = await _userModelFactory.PrepareProfileModelAsync(user);
            return Success(model, "Success.");
        }
        else if (currentStudent != null)
        {
            var student = await _registrationMasterService.GetRegistrationMasterByIdAsync(currentStudent.Id);
            if (student == null)
                return Error("", "User not found");

            var model = await _projectGyaanRegistrationModelFactory.PrepareProjectGyaanRegistrationModelAsync(null, student);
            return Success(model, "Success.");
        }
        return Error("", "User not found");
    }

    [HttpPost]
    public async Task<IActionResult> ProfileEdit([FromBody] UserProfileModel model)
    {
        var currentUser = await _workContext.GetCurrentUserAsync();
        var currentStudent = await _workContext.GetCurrentStudentsync();

        // Show profile data as per role
        if (currentUser != null)
        {
            var user = await _userService.GetUserByIdAsync(currentUser.Id);
            if (user == null)
                return Error("", "User not found");

            //var user = await _
            var userdetails = await _userService.GetExistUserAsync(model.EmailAddress, model.MobileNumber, currentUser.Id);
            if (userdetails != null)
                return Error(string.Empty, "This mail or mobile number is already in use.");

            user = model.ToEntity(currentUser);
            user.UpdatedOnUtc = DateTime.Now;
            user.Password = EncryptionHelper.EncryptPassword(model.Password);
            await _userService.UpdateUserAsync(currentUser);
            return Success(currentUser, "Profile updated successfully.");
        }
        else if (currentStudent != null)
        {
            var student = await _registrationMasterService.GetRegistrationMasterByIdAsync(currentStudent.Id);
            if (student == null)
                return Error("", "User not found");

            var existingUser = await _registrationMasterService.CheckCandidateExistForUpdateAsync(model.EmailAddress, model.MobileNumber, currentStudent.Id);
            if (existingUser != null)
                return Error(existingUser, "This email address & mobile number already exist.");

            student = model.ToEntity(student);
            student.UpdatedBy = "System";
            student.UpdatedOn = DateTime.UtcNow;
            student.Password = EncryptionHelper.EncryptPassword(model.Password);
            await _registrationMasterService.UpdateRegistrationMasterAsync(student);

            return Success(student, "Profile updated successfully.");
        }
        return Error("", "User not found");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserList([FromQuery] UserSearchModel searchModel)
    {
        //prepare department list model
        var query = await _userModelFactory.PrepareUserListModelAsync(searchModel);

        if (query == null)
            return Error("", "Data not found.");

        return PagedList(query);
    }


    [HttpGet]
    public async Task<IActionResult> GetUserDetails(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return Error("", "User not found.");

        var model = await _userModelFactory.PrepareUserModelAsync(null, user);

        return Success(model, "Success");
    }

    [HttpGet]
    public virtual async Task<IActionResult> ExportAllUsersDetailXlsx([FromQuery] UserSearchModel searchModel)
    {
        try
        {
            var pagedUsers = await _userService.GetAllUsersAsync(searchText: searchModel.SearchText, pageIndex: 0, pageSize: int.MaxValue);

            var users = pagedUsers.ToList();
            var userExcel = await _exportManager.ExportAllUsersDetailToXlsxAsync(users);

            var fileName = $"Candidates {DateTime.Now:ddMMyyyy}.xlsx";
            return File(userExcel, MimeTypes.TextXlsx, fileName);

        }
        catch (Exception exc)
        {
            return Error(exc.Message);
        }
    }

    #endregion
}   
