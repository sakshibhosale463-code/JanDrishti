using Project.Core;

namespace Project.Services.Report.EntityModel;
public class DashBoardEntityModel : BaseEntity
{
    // Admin
    public long RegistrationsCount { get; set; }
    public long AssessmentSubmissions { get; set; }
    public long TotalUsers { get; set; }
    public long TotalBatches { get; set; }
    public long TotalSessions { get; set; }
    public long TotalTasks { get; set; }
    public long CompletedTasks { get; set; }
    public long TotalStudents { get; set; }
    public long TotalBatchEnrolledStudents { get; set; }

    // Trainer (reusing same fields for simplicity)
    // TotalSessions, TotalTasks, TotalStudents, CompletedTasks, TotalBatches, TotalBatchEnrolledStudents

    // Student
    public long TotalAssignedTasks { get; set; }
    // CompletedTasks already included above
    // TotalBatches already included above
}