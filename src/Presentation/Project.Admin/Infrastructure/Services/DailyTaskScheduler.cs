using Microsoft.Extensions.Hosting;
using Project.Core.Domain.Candidate;

namespace Project.Admin.Infrastructure.Services;

public class DailyTaskScheduler : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromHours(1); // run every 1 hours

    public DailyTaskScheduler()
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    while (!stoppingToken.IsCancellationRequested)
    //    {
    //        try
    //        {
    //            await RunScheduledTaskAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            // Log exception (replace with your logging)
    //            Console.WriteLine($"Error in DailyTaskScheduler: {ex.Message}");
    //        }

    //        // Wait for next run
    //        await Task.Delay(_interval, stoppingToken);
    //    }
    //}

    //private async Task RunScheduledTaskAsync()
    //{
    //    Console.WriteLine($"DailyTaskScheduler running at {DateTime.Now}");

    //    // Get sessions that need attendance
    //    var sessionsToday = await _sessionService.GetToDaySession();
    //    if (sessionsToday == null || !sessionsToday.Any())
    //        return;

    //    foreach (var session in sessionsToday)
    //    {
    //        // Session must exist & not deleted
    //        if (session == null || session.Deleted)
    //            continue;

    //        // SKIP EXTRA SESSIONS
    //        if (session.IsExtra)
    //            continue;

    //        //  Get students in batch
    //        var students = await _attendanceService.GetCandidateListByBatchIdAsync(session.BatchId);
    //        if (students == null || !students.Any())
    //            continue;

    //        // Get existing attendance once
    //        var existingAttendance = await _attendanceService.GetAttendanceBySessionIdAsync(session.Id);
    //        var existingStudentIds = existingAttendance
    //            .Where(x => !x.Deleted)
    //            .Select(x => x.RegisterId)
    //            .ToHashSet();

    //        foreach (var student in students)
    //        {
    //            // Validate student
    //            if (student == null || student.StudentId <= 0)
    //                continue;

    //            // Prevent duplicate (in-memory)
    //            if (existingStudentIds.Contains(student.StudentId))
    //                continue;

    //            var attendance = new Attendance
    //            {
    //                SessionId = session.Id,
    //                RegisterId = student.StudentId,
    //                Status = "Absent",
    //                Deleted = false,
    //                PresentDate = session.SessionDate.Date
    //            };

    //            await _attendanceService.InsertAttendanceAsync(attendance);

    //            // Prevent duplicate in same run
    //            existingStudentIds.Add(student.StudentId);
    //        }
    //    }

    //    Console.WriteLine($"DailyTaskScheduler finished at {DateTime.Now}");
    //}

}