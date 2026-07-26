using System.Data;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using LinqToDB.Data;
using Project.Data;
using Project.Services.Report.EntityModel;

namespace Project.Services.Report;
public class ReportService : IReportService
{
    #region Fields

    private readonly IRepository<DashBoardEntityModel> _dashBoardEntityModel;
    private readonly IRepository<AttendanceEntityModel> _attendanceEntityModel;

    #endregion

    #region Constructor

    public ReportService(IRepository<DashBoardEntityModel> dashBoardEntityModel,
                     IRepository<AttendanceEntityModel> attendanceEntityModel)
    {
        _dashBoardEntityModel = dashBoardEntityModel;
        _attendanceEntityModel = attendanceEntityModel;
    }

    #endregion


    #region Methods

    public async Task<DashBoardEntityModel> GetDashBoardCounts(long roleId,long userId,DateTime? fromDate = null, DateTime? toDate = null)
    {
        if (toDate.HasValue && toDate != DateTime.MinValue)
        {
            toDate = toDate.Value.AddDays(1);
        }

        var parameterList = new List<DataParameter>
        {
            new DataParameter("@RoleId", roleId),
            new DataParameter("@LoginUserId", userId),
            new DataParameter("@FromDate", fromDate),
            new DataParameter("@ToDate", toDate)
        };

        var data = await _dashBoardEntityModel.EntityFromSqlAsync("spGetDashboardCounts", parameterList.ToArray());
        return data.FirstOrDefault();
    }


    public async Task<AttendanceEntityModel> GetAttendanceCounts(long sessionId, long registerId, string filterType = "Today")
    {
        var parameterList = new List<DataParameter>
        {
            new DataParameter("@SessionId", sessionId),
            new DataParameter("@RegisterId", registerId),
            new DataParameter("@FilterType", filterType),
        };

        var data = await _attendanceEntityModel.EntityFromSqlAsync("spGetAttendanceCounts", parameterList.ToArray());
        return (await data.ToListAsync()).FirstOrDefault() ?? new AttendanceEntityModel();
    }


    public async Task<DashboardCountEntityModel> PrepareDashboardCountListAsync(long roleId, long sessionId = 0, long userId = 0, 
        string filterType = "Today", DateTime? fromDate = null, DateTime? toDate = null)
    {
        var attendanceTask = GetAttendanceCounts(sessionId, userId, filterType);
        var dashboardTask = GetDashBoardCounts(roleId, userId, fromDate, toDate);

        await Task.WhenAll(attendanceTask, dashboardTask);

        return new DashboardCountEntityModel
        {
            AttendanceCounts = await attendanceTask,
            DashboardCounts = await dashboardTask
        };
    }

    #endregion
}
