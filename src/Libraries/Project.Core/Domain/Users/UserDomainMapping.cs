namespace Project.Core.Domain.Users;
public partial class UserDomainMapping:BaseEntity
{
    public long UserId { get; set; }
    public long DomainId { get; set; }
}
