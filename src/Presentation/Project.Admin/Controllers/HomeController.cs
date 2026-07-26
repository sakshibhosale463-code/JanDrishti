using Microsoft.AspNetCore.Mvc;
using Project.Core;
using Project.Core.Domain.Users;
using Project.Services.Authentication;
using Project.Services.ExportImport;
using Project.Services.Users;

namespace Project.Admin.Controllers;

public class HomeController : Controller
{
    private readonly IWorkContext _workContext;
    private readonly IUserService _userService;
    private readonly IImportManager _importManager;
    private readonly IAuthenticationService _authenticationService;

    public HomeController(IWorkContext workContext, IUserService userService, IAuthenticationService authenticationService, IImportManager importManager)
    {
        _workContext = workContext;
        _userService = userService;
        _importManager = importManager;
        _authenticationService = authenticationService;
    }

    public IActionResult Index()
    {
        string htmlContent = "";
        return Content(htmlContent, "text/html");
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        var user = await _workContext.GetCurrentUserAsync();
        return Ok(user);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ImportFromXlsx(IFormFile importexcelfile)
    {
        try
        {
            if (importexcelfile != null && importexcelfile.Length > 0)
            {
                await _importManager.ImportUsersFromXlsxAsync(importexcelfile.OpenReadStream());
            }
            else
            {
                //_notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Common.UploadFile"));
                return RedirectToAction("List");
            }

            //_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Manufacturers.Imported"));
            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            return RedirectToAction("List");
        }
    }
}
