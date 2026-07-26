using Project.Services.Report.EntityModel;

namespace Project.Services.Report;
public interface IReportService
{
    Task<DashBoardEntityModel> GetDashBoardCounts(long roleId, long userId, DateTime? fromDate = null, DateTime? toDate = null);

    Task<AttendanceEntityModel> GetAttendanceCounts(long sessionId, long registerId, string filterType = "Today");

    Task<DashboardCountEntityModel> PrepareDashboardCountListAsync(long roleId, long sessionId = 0, long userId = 0, string filterType = "Today", DateTime? fromDate = null, DateTime? toDate = null);
}
