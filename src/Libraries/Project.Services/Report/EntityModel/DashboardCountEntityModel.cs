namespace Project.Services.Report.EntityModel;
public class DashboardCountEntityModel
{
    public DashBoardEntityModel DashboardCounts { get; set; }
    public AttendanceEntityModel AttendanceCounts { get; set; }

    public DashboardCountEntityModel()
    {
        DashboardCounts = new DashBoardEntityModel();
        AttendanceCounts = new AttendanceEntityModel();
    }

}
