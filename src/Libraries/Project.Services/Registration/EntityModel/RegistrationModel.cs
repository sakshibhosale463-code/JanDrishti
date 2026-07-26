using Project.Core;

namespace Project.Services.Registration.EntityModel;
public class RegistrationModel:BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public long CurrentStatus { get; set; }
    public string CurrentStatusName { get; set; }
    public string CollageOrUniversityName { get; set; }
    public string CourceOrDegree { get; set; }
    public string YearOfStudyPassingYear { get; set; }
    public bool AvailableForWeekendSession { get; set; }
    public int AssessmentStatus { get; set; }
    public bool IsPayment { get; set; }
    public int PaymentStatus { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    //public long InternShipTypeId { get; set; }
    public string AvailableForWeekendSessionName => AvailableForWeekendSession == true ? "Yes" : "No";
    public bool AvailableForOneToOneSession { get; set; }
    public string AvailableForOneToOneSessionName => AvailableForOneToOneSession == true ? "Yes" : "No";

    public bool IsActive { get; set; }
    public long DomainId { get; set; }
    public decimal PaidAmount { get; set; }
}
