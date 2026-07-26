using Project.Web.Framework.Models;

namespace Project.Admin.Area.Model;

public partial record UserProfileModel : BaseModel
{
    public string Name { get; set; }
    public string MobileNumber { get; set; }
    public string EmailAddress { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public string Role { get; set; }
    public string SystemName { get; set; }
    public string FullName { get; set; }
    //public bool Active { get; set; }
    public long RoleId { get; set; }
    public string Remark { get; set; }
    public string Password { get; set; }
    public List<string> Domains { get; set; }


    public string CollageOrUniversityName { get; set; }
    public string CourceOrDegree { get; set; }
    public string YearOfStudyPassingYear { get; set; }
    public long CurrentStatus { get; set; }

}
