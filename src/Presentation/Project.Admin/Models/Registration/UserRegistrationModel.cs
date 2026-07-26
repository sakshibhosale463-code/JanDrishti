using Project.Web.Framework.Models;

namespace Project.Admin.Models.Registration;

public partial record UserRegistrationModel: BaseModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string MobileNumber { get; set; }
    public long CurrentStatus { get; set; }
    public string CollageOrUniversityName { get; set; }
    public string CourceOrDegree { get; set; }
    public string YearOfStudyPassingYear { get; set; }
    public bool AvailableForWeekendSession { get; set; }
    public bool AvailableForOneToOneSession { get; set; }
    public string CurrentStatusName { get; set; }
    public int AssessmentStatus { get; set; }
    public bool IsPayment { get; set; }
    public int PaymentStatus { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public long DomainId { get; set; }
    public string Cource { get; set; }
    public string Role { get; set; }
    public long RoleId { get; set; }
    public string SystemName { get; set; }
    public string AvailableForWeekendSessionName => AvailableForWeekendSession == true ? "Yes" : "No";
    public string AvailableForOneToOneSessionName => AvailableForOneToOneSession == true ? "Yes" : "No";

    public bool IsActive { get; set; }
    public decimal PaidAmount { get; set; }
}
