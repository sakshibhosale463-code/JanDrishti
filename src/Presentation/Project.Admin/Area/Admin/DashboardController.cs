using Microsoft.AspNetCore.Mvc;
using Project.Admin.Area.Model;
using Project.Core;
using Project.Core.Domain.Users;
using Project.Services.Catalog;
using Project.Services.Report;
using Project.Services.Users;
using Project.Web.Framework;
namespace Project.Admin.Area.Admin;
public class DashboardController : BaseProtectedController
{
    #region Fields

    private readonly IWorkContext _workContext;
    private readonly IUserService _userService;
    private readonly IReportService _reportService;
    private readonly IUserRoleService _userRoleService;
    #endregion

    #region Contructor
    public DashboardController(IWorkContext workContext,
        IUserService userService, IReportService reportService,
        IUserRoleService userRoleService)
    {
        _workContext = workContext;
        _userService = userService;
        _reportService = reportService;
        _userRoleService = userRoleService;
    }


    #endregion

    #region Methods

    [HttpGet]
    public async Task<IActionResult> GetDashBoardDetails([FromQuery] SearchModel searchModel, [FromQuery] bool includeGraph = false, [FromQuery] string filterType = "Today")
    {
        var currentUser = await _workContext.GetCurrentUserAsync();
        return Success("Success");
    }

    #endregion
}
