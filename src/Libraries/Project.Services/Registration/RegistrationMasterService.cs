using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using Project.Core;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Users;
using Project.Data;
using Project.Core.Domain.Candidate;
namespace Project.Services.Registration;
public class RegistrationMasterService : IRegistrationMasterService
{
    #region Fields
    private readonly IRepository<RegistrationMaster> _registrationMasterRepository;
    private readonly IRepository<User> _userRepository;
    #endregion

    #region Constructor

    public RegistrationMasterService(
        IRepository<RegistrationMaster> registrationMasterRepository,
        IRepository<User> userRepository)
    {
        _registrationMasterRepository = registrationMasterRepository;
        _userRepository = userRepository;
    }

    #endregion

    #region Methods Registration

    /// <summary>
    /// Gets a user
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task<RegistrationMaster> GetRegistrationMasterByIdAsync(long id)
    {
        return await _registrationMasterRepository.GetByIdAsync(id);
    }
    public async Task<RegistrationMaster> GetRegistrationMasterByEmailIdAsync(string email)
    {
        return await _registrationMasterRepository.Table.FirstOrDefaultAsync(c=>c.Email==email);
    }

    /// <summary>
    /// Insert user
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task<long> InsertRegistrationMasterAsync(RegistrationMaster user)
    {
        await _registrationMasterRepository.InsertAsync(user);
        return user.Id;
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task UpdateRegistrationMasterAsync(RegistrationMaster user)
    {
        await _registrationMasterRepository.UpdateAsync(user);
    }


    public async Task<RegistrationMaster> CheckCandidateExistAsync(string email, string mobileNumber)
    {
        var query = await _registrationMasterRepository.Table.FirstOrDefaultAsync(x => !x.Deleted && (x.Email == email || x.MobileNumber == mobileNumber));
        return query;
    }

    public async Task<RegistrationMaster> ValidateStudentNameAndPasswordAsync(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentNullException(nameof(userName));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        // add by Avinash for login with universe database password

        var hashPasswords = KeyPasswordSet(password);
        var query = _registrationMasterRepository.Table;
        var student = await query.FirstOrDefaultAsync(user => user.Email == userName && user.Password == password || user.Password == hashPasswords && user.Deleted == false);

        return student;
    }


    /// <summary>
    /// Converts a string to a custom encoded format by converting each character to its ASCII value,
    /// padding it to three digits, and then reversing the entire string.
    /// </summary>
    /// <param name="sValue">The input string to encode.</param>
    /// <returns>The encoded string.</returns>
    public string KeyPasswordSet(string sValue)
    {
        string strR = "";
        foreach (char c in sValue)
        {
            int intTemp = (int)c;
            string strTemp1;

            if (intTemp <= 9)
                strTemp1 = "00";
            else if (intTemp <= 99)
                strTemp1 = "0";
            else
                strTemp1 = "";

            strR += strTemp1 + intTemp.ToString();
        }

        return new string(strR.Reverse().ToArray());
    }

    public async Task<RegistrationMaster> CheckCandidateExistForUpdateAsync(string email, string mobileNumber, long id)
    {
        var query = await _registrationMasterRepository.Table.FirstOrDefaultAsync(x => x.Id != id && !x.Deleted && (x.Email == email || x.MobileNumber == mobileNumber));
        return query;
    }

    public async Task<bool> IsEmailOrMobileExistsForUpdate(string email, string mobile, long? userId = null, long? studentId = null)
    {
        email = email?.Trim().ToLower();
        mobile = mobile?.Trim();

        // Check in Users
        var user = await _userRepository.Table.FirstOrDefaultAsync(x => !x.Deleted && (x.EmailAddress == email || x.MobileNumber == mobile) && (!userId.HasValue || x.Id != userId.Value));
        if (user != null)
            return true;

        // Check in Students
        var student = await _registrationMasterRepository.Table.FirstOrDefaultAsync(x => !x.Deleted && (x.Email == email || x.MobileNumber == mobile) && (!studentId.HasValue || x.Id != studentId.Value));
        if (student != null)
            return true;

        return false;
    }


    #endregion

}
