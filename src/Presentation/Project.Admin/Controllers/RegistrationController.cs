using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Project.Admin.Area.Model;
using Project.Admin.Factories;
using Project.Admin.Hubs;
using Project.Admin.Models.Notification;
using Project.Admin.Models.Registration;
using Project.Admin.Models.Users;
using Project.Core;
using Project.Core.Configuration;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Master;
using Project.Core.Http;
using Project.Core.Infrastructure;
using Project.Core.Security;
using Project.Data;
using Project.Services.Catalog;
using Project.Services.Email;
using Project.Services.ExportImport;
using Project.Services.Registration;
using Project.Services.Users;
using Project.Web.Framework;
using Project.Web.Framework.Infrastructure.Mapper.Extensions;

namespace Project.Admin.Controllers;

public class RegistrationController : BaseProtectedController
{
    #region Fields

    private readonly IConfiguration _configuration;
    IRegistrationMasterService _registrationMasterService;
    private IRegistrationModelFactory _registrationModelFactory;
    private readonly IExportManager _exportManager;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IHttpService _httpService;
    private readonly IWorkContext _workContext;
    private readonly IUserRoleService _userRoleService;
    private readonly ILogger<RegistrationController> _logger;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IRepository<NotificationMaster> _notificationRepository;

    #endregion

    #region Constr

    public RegistrationController(IConfiguration configuration,
        IRegistrationMasterService registrationMasterService, 
        IExportManager exportManager,
        IRegistrationModelFactory registrationModelFactory,
        IUserService userService,
        IEmailService emailService,
        IHttpService httpService,
        IWorkContext workContext,
        IUserRoleService userRoleService,
        ILogger<RegistrationController> logger,
        AppSettings settings,
        IHubContext<ChatHub> hubContext,
        IRepository<NotificationMaster> notificationRepository)
    {
        _configuration = configuration;
        _exportManager = exportManager;
        _registrationModelFactory = registrationModelFactory;
        _registrationMasterService = registrationMasterService;
        _userService = userService;
        _emailService = emailService;
        _httpService = httpService;
        _workContext = workContext;
        _userRoleService = userRoleService;
        _logger = logger;
        _hubContext = hubContext;
        _notificationRepository = notificationRepository;
    }

    #endregion

    #region Utilities


    public static string GenerateSecureToken(int length = 32)
    {
        const string allowedChars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = allowedChars[bytes[i] % allowedChars.Length];
        }

        return new string(chars);
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

    #endregion

    #region Registraton Method

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Registration(UserRegistrationModel model)
    {
        if (model == null)
            return Error("", "Model should be data");

        if (model.DomainId == 0)
            return Error("", "Domain is required.");

        //Check user email already exist
        var exists = await IsEmailOrMobileExists(model.Email, model.MobileNumber);
        if (exists)
            return Error("", "Email or mobile number already exists.");

        // Insert Candidate
        var data = model.ToEntity<RegistrationMaster>();
        data.CreatedBy = "System";
        data.CreatedOn = DateTime.Now;
        data.IsActive = false;
        data.IsPayment = false;
        data.PaymentStatus = (int)PaymentStatusEnum.Pending;
        data.PaidAmount = 0;
        data.AssessmentStatus = (int)AssessmentStatusEnum.Pending;
        data.Password = EncryptionHelper.EncryptPassword(model.Password);
        var registrationId = await _registrationMasterService.InsertRegistrationMasterAsync(data);

        var notification = new NotificationMaster
        {
            Title = "New Registration",
            Message = $"{data.FullName} has registered for Project Gyaan.",
            NotificationType = "ProjectGyaanRegistrationCreated",
            IsRead = false,
            CreatedOnUtc = DateTime.Now
        };

        await _notificationRepository.InsertAsync(notification);

        await _hubContext.Clients.All.SendAsync("ProjectGyaanRegistrationCreated", new
        {
            data.Id,
            data.FullName,
            data.Email,
            data.MobileNumber,
            data.CreatedOn
        });
        return Success(new { registrationId = registrationId }, "Registration successful.");
    }

    //[AllowAnonymous]
    //[HttpGet]
    //public async Task<IActionResult> GetAllRegistration([FromQuery] RegistrationSearchModel searchModel, bool? onlyPaidUsers = null)
    //{
    //    var data = await _registrationModelFactory.PrepareProjectGyaanRegistrationListModelAsync(searchModel, onlyPaidUsers);
    //    if (data == null)
    //        return Success("", "Data not found.");

    //    return PagedList(data, "Success.");
    //}

    #region Email
    public Task<bool> ProjectGyaanRegistrationEmail(string email, string name)
    {
        var password = $"{_configuration["emailsettings:password"]}";
        var emailid = $"{_configuration["emailsettings:email"]}";
        string htmlBody = $@"
        <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <title>Registration Confirmation – Project Gyaan</title>
            </head>
            <body style=""font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #333; line-height: 1.6;"">
                <p>Dear <strong>{name}</strong>,</p>
                <p>
                    Thank you for registering for <strong>Project Gyaan</strong>.
                </p>
                <p>
                    We have successfully received your application. Our team will review your details
                    and contact you shortly with the next steps.
                </p>
                <p>
                    If you have any questions, feel free to reach out to us.
                </p>
                <p>
                    Thank You,<br>
                    <strong>Team Project Gyaan</strong><br>
                    Email: info@projectgyaan.com
                </p>
            </body>
            </html>";
        var toemails = new List<string>();
        string subject = $"Registration Confirmation – Project Gyaan";

        var toRecipients = new List<string>();
        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailid),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        using (var smtpClient = new SmtpClient("smtp.office365.com"))
        {
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailid, password);
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        return Task.FromResult(true);
    }


    #endregion

    #endregion

    #region Admin side Registration Apis

    //[HttpGet]
    //public virtual async Task<IActionResult> ExportAllRegistrationDetailsXlsx([FromQuery] UserSearchModel searchModel)
    //{
    //    try
    //    {
    //        var registration = await _registrationMasterService.GetAllRegistrationUsersListAsync(searchText: searchModel.SearchText);

    //        var userExcel = await _exportManager.ExportAllCandidateRegistrationDetailsToXlsxAsync(registration);
    //        var fileName = $"Candidates {DateTime.Now:ddMMyyyy}.xlsx";
    //        return File(userExcel, MimeTypes.TextXlsx, fileName);

    //        //var fileName = $"Customer_Registration_List {DateTime.Now:ddMMyyyyHHmm}.xlsx";
    //        //return File(userExcel, MimeTypes.TextXlsx, fileName);
    //    }
    //    catch (Exception exc)
    //    {
    //        return Error(exc.Message);
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> DeleteRegistration([FromBody] RegistrationMaster model)
    {
        var regitration = await _registrationMasterService.GetRegistrationMasterByIdAsync(model.Id);
        if (regitration == null || regitration.Deleted)
            return Error("User not found.");

       // await _registrationMasterService.DeleteRegistrationAsync(regitration);

        return Success(model.Id, "Registration deleted successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetProjectGyaanRegistrationDetails(long id)
    {
        var registration = await _registrationMasterService.GetRegistrationMasterByIdAsync(id);
        if (registration == null)
            return Error("", "User not found.");

        //prepare model
        var model = await _registrationModelFactory.PrepareProjectGyaanRegistrationModelAsync(null, registration);
        return Success(model, "Success");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProjectGyaanRegistration(UserRegistrationModel model)
    {
        if (model == null)
            return Error("", "Model should be data");

        var exists = await _registrationMasterService.IsEmailOrMobileExistsForUpdate(model.Email, model.MobileNumber, userId: null, studentId: model.Id);
        if (exists)
            return Error("", "Email or mobile number already exists.");

        var registration = await _registrationMasterService.GetRegistrationMasterByIdAsync(model.Id);
        if (registration == null)
            return Error("", "User not found.");

        registration = model.ToEntity(registration);
        registration.IsPayment = true;
        registration.PaymentStatus = (int)PaymentStatusEnum.Paid;
        registration.PaidAmount = model.PaidAmount;
        registration.UpdatedBy = "System";
        registration.UpdatedOn = DateTime.Now;
        registration.Password = EncryptionHelper.EncryptPassword(model.Password);
        await _registrationMasterService.UpdateRegistrationMasterAsync(registration);

        return Success(registration, "Registration updated successfully.");
    }


    #endregion

    #region Notification Store

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllNotifications()
    {
        var data = await _notificationRepository.Table.OrderByDescending(x => x.Id)
            .Select(x => new NotificationModel
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                NotificationType = x.NotificationType,
                IsRead = x.IsRead,
                CreatedOnUtc = x.CreatedOnUtc
            }).ToListAsync();

        return Success(data, "Notifications found successfully");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetUnreadNotificationCount()
    {
        var count = await _notificationRepository.Table.CountAsync(x => !x.IsRead);

        return Success(count, "Unread notification count");
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ReadNotification([FromBody] NotificationModel model)
    {
        if (model == null || model.Id <= 0)
            return Error("", "Invalid request");

        var notification = await _notificationRepository.GetByIdAsync(model.Id);

        if (notification == null)
            return Error("", "Notification not found");

        notification.IsRead = true;

        await _notificationRepository.UpdateAsync(notification);

        return Success(notification.Id, "Notification marked as read");
    }

    #endregion
}

