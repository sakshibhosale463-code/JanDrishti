using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Admin.Factories;
using Project.Admin.Models.Users;
using Project.Core.Configuration;
using Project.Core.Infrastructure;
using Project.Core.Security;
using Project.Services.Catalog;
using Project.Services.Email;
using Project.Services.Registration;
using Project.Services.Users;
using Project.Web.Framework;

namespace Project.Admin.Controllers;

public class AuthController : BaseController
{
    #region Fields

    private readonly IUserService _userService;
    private readonly IUserModelFactory _userModelFactory;
    private readonly IUserRoleService _userRoleService;
    private readonly IRegistrationMasterService _registrationMasterService;
    private readonly IRegistrationModelFactory _projectGyaanRegistrationModelFactory;
    private readonly IEmailService _emailService;
    #endregion

    #region Constructor

    public AuthController(IUserService userService,
        IUserRoleService userRoleService,
        IUserModelFactory userModelFactory,
        IRegistrationMasterService registrationMasterService, 
        IRegistrationModelFactory projectGyaanRegistrationModelFactory,
        IEmailService emailService)
    {
        _userService = userService;
        _userRoleService = userRoleService;
        _userModelFactory = userModelFactory;
        _registrationMasterService = registrationMasterService;
        _projectGyaanRegistrationModelFactory = projectGyaanRegistrationModelFactory;
        _emailService = emailService;
    }

    #endregion

    #region Methods

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (model == null)
            return Error(string.Empty, "Invalid request");

        model.Password = EncryptionHelper.EncryptPassword(model.Password);

        // First try User (Admin/Trainer)
        var user = await _userService.ValidateUserNameAndPasswordAsync(model.UserName, model.Password);

        if (user != null)
        {
            if (user.Deleted)
                return Error(string.Empty,
                    "The user account has been deleted. Please contact administrator.");

            if (!user.Active)
                return Error(string.Empty,
                    "The user account is not active. Please contact administrator.");

            var authModel = await _userModelFactory.PrepareUserAuthModelAsync(user);

            return Success(authModel, "Login successful");
        }

        // If not found in User, try Student
        var student = await _registrationMasterService.ValidateStudentNameAndPasswordAsync(model.UserName, model.Password);

        if (student != null)
        {
            if (student.Deleted)
                return Error(string.Empty,
                    "The student account has been deleted. Please contact support.");

            if (!student.IsActive)
                return Error(string.Empty,
                    "The student account is not active. Please contact support.");

            var authModel = await _projectGyaanRegistrationModelFactory.PrepareStudentAuthModelAsync(student);

            return Success(authModel, "Login successful");
        }

        return Error(string.Empty, "User credentials are not valid");
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ResetPasswordRequestModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Email))
            return Error("Please enter a valid email address.");

        var settings = Singleton<AppSettings>.Instance.Get<CommonConfig>();
        var token = Guid.NewGuid().ToString();
        var resetLink = $"{settings.ReactBaseUrl}{token}";

        string userName = string.Empty;
        string email = model.Email;

        //  Check Student Table First
        var student = await _registrationMasterService.GetRegistrationMasterByEmailIdAsync(model.Email);
        if (student != null)
        {
            student.ResetToken = token;
            student.ResetTokenExpiry = DateTime.Now.AddHours(24);
            await _registrationMasterService.UpdateRegistrationMasterAsync(student);

            userName = student.FullName;
        }
        else
        {
            //  Check Users Table
            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null)
                return Error("This email is not registered. Please enter a registered email.");

            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddHours(24);
            await _userService.UpdateUserAsync(user);

            userName = user.Name;
        }

        //  Send Email (Common for both)
        var toEmailList = new List<string>();
        if (!string.IsNullOrWhiteSpace(email))
            toEmailList.Add(email);

        var emailTemplate = await _userService.GetEmailTemplateByNameAsync("ForgotPassword");
        var sb = new StringBuilder(emailTemplate.Body);
        sb.Replace("%ResetLink%", resetLink)
          .Replace("%UserName%", userName);

        string body = sb.ToString();
        await _emailService.SendEmailAsync(toEmailList, emailTemplate.Subject, body);
        return Success(token, "Password reset link sent successfully.");
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> CheckResetUrlValide(string resetTocken)
    {
        if (string.IsNullOrWhiteSpace(resetTocken))
            return Error("Reset token cannot be null or empty.");

        var (isValid, message, email, userType) =
            await _userService.CheckResetTokenValidAsync(resetTocken);

        if (!isValid)
            return Error(message);

        return Success(new
        {
            IsValid = isValid,
            Email = email,
            UserType = userType  
        }, message);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordLink([FromBody] ResetPasswordRequestModel model)
    {
        if (string.IsNullOrWhiteSpace(model.ResetToken))
            return Error("Invalid reset link.");

        var (isValid, message, email, userType) = await _userService.CheckResetTokenValidAsync(model.ResetToken);

        if (!isValid)
            return Error(message);

        if (userType == "User")
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return Error("User not found.");

            user.Password = EncryptionHelper.EncryptPassword(model.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userService.UpdateUserAsync(user);
        }
        else if (userType == "Student")
        {
            var student = await _registrationMasterService
                .GetRegistrationMasterByEmailIdAsync(email);

            if (student == null)
                return Error("Student not found.");

            student.Password = EncryptionHelper.EncryptPassword(model.NewPassword);
            student.ResetToken = null;
            student.ResetTokenExpiry = null;

            await _registrationMasterService.UpdateRegistrationMasterAsync(student);
        }
        else
        {
            return Error("Invalid user type.");
        }

        return Success(true, "Password updated successfully.");
    }


    #endregion
}
