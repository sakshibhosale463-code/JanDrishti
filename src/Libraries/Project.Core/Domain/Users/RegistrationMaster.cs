using Project.Core.Domain.Catalog;
using Project.Core.Domain.Common;

namespace Project.Core.Domain.Candidate;
public partial class RegistrationMaster : BaseEntity, ISoftDeletedEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
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
}
