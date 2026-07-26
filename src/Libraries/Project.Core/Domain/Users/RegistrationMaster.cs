using Project.Core.Domain.Catalog;
using Project.Core.Domain.Common;

namespace Project.Core.Domain.Candidate;
public partial class RegistrationMaster : BaseEntity, ISoftDeletedEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public long CurrentStatus { get; set; }
    public string CollageOrUniversityName { get; set; }
    public string CourceOrDegree { get; set; }
    public string YearOfStudyPassingYear { get; set; }
    public bool AvailableForWeekendSession { get; set; }
    public bool AvailableForOneToOneSession { get; set; }
    //public AssessmentStatusEnum AssessmentStatus { get; set; }
    public AssessmentStatusEnum AssessmentStatusEnum
    {
        get => (AssessmentStatusEnum)AssessmentStatus;
        set => AssessmentStatus = (int)value;
    }
    public int AssessmentStatus { get; set; }

    //public bool PaymentStatus { get; set; }
    public bool IsPayment { get; set; }
    public long PaymentStatus { get; set; }
    public PaymentStatusEnum PaymentStatusEnum
    {
        get => (PaymentStatusEnum)PaymentStatus;
        set => PaymentStatus = (int)value;
    }

    public bool Deleted { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string? Password { get; set; }
    public long RoleId { get; set; }

    public string ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public string UserCode { get; set; }

    public decimal PaidAmount { get; set; } = 0;
   
}
