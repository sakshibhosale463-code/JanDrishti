using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Core;
using Project.Core.Domain.Candidate;
namespace Project.Services.Registration;
public interface IRegistrationMasterService
{
    #region Registration
    Task<RegistrationMaster> GetRegistrationMasterByEmailIdAsync(string email);
    Task<RegistrationMaster> GetRegistrationMasterByIdAsync(long id);
    Task UpdateRegistrationMasterAsync(RegistrationMaster user);
    Task<long> InsertRegistrationMasterAsync(RegistrationMaster user);
    Task<RegistrationMaster> ValidateStudentNameAndPasswordAsync(string userName, string password);
    Task<RegistrationMaster> CheckCandidateExistAsync(string email, string mobileNumber);
    Task<RegistrationMaster> CheckCandidateExistForUpdateAsync(string email, string mobileNumber, long id);
    Task<bool> IsEmailOrMobileExistsForUpdate(string email, string mobile, long? userId = null, long? studentId = null);

    #endregion

}
