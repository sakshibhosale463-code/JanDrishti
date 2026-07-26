using System.Data;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Core;
using Project.Core.Caching;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Security;
using Project.Core.Domain.Users;
using Project.Data;
using Project.Services.Databases;

namespace Project.Services.Users;

/// <summary>
/// Represents user service
/// </summary>
public class UserService : IUserService
{
    #region Fields

    private readonly IRepository<User> _userRepository;
    private readonly IProjectDataProvider _projectDataProvider;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<PermissionRecord> _permissionRecordRepository;
    private readonly IRepository<Department> _departmentRepository;
    private readonly IRepository<UserDomainMapping> _userDomainMappingRepository;
    private readonly IRepository<MessageTemplate> _messageTemplateRepository;
    private readonly IRepository<RegistrationMaster> _registrationMasterRepository;
    #endregion

    #region Constructor

    public UserService(IRepository<User> userRepository,
        IProjectDataProvider projectDataProvider,
        IStaticCacheManager staticCacheManager,
        IRepository<UserRole> userRoleRepository,
        IRepository<PermissionRecord> permissionRecordRepository,
        IRepository<Department> departmentRepository,
        IRepository<UserDomainMapping> userDomainMappingRepository,
        IRepository<MessageTemplate> messageTemplateRepository,
        IRepository<RegistrationMaster> registrationMasterRepository)
    {
        _userRepository = userRepository;
        _projectDataProvider = projectDataProvider;
        _staticCacheManager = staticCacheManager;
        _userRoleRepository = userRoleRepository;
        _permissionRecordRepository = permissionRecordRepository;
        _departmentRepository = departmentRepository;
        _userDomainMappingRepository = userDomainMappingRepository;
        _messageTemplateRepository = messageTemplateRepository;
        _registrationMasterRepository = registrationMasterRepository;
    }

    #endregion

    #region Methods User

    /// <summary>
    ///  Gets a user by code
    /// </summary>
    /// <param name="code"></param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the User
    /// </returns>
    public async Task<User> GetUserByEmailAsync(string email, string mobileNumber)
    {
        return await _userRepository.Table.FirstOrDefaultAsync(c => !c.Deleted && (c.EmailAddress == email || c.MobileNumber == mobileNumber));
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userRepository.Table.FirstOrDefaultAsync(c => c.EmailAddress == email && !c.Deleted);
    }


    /// <summary>
    ///  Gets a user by code
    /// </summary>
    /// <param name="code"></param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the User
    /// </returns>
    public async Task<User> GetExistUserAsync(string email, string mobileNumber, long id)
    {
        return await _userRepository.Table.FirstOrDefaultAsync(x => x.Id != id && !x.Deleted && (x.EmailAddress == email || x.MobileNumber == mobileNumber));
    }

    /// <summary>
    /// Gets a user
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task<User> GetUserByIdAsync(long userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    /// <summary>
    /// Validate user name and password
    /// </summary>
    /// <param name="code">Username</param>
    /// <param name="password">Password</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task<User> ValidateUserNameAndPasswordAsync(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentNullException(nameof(userName));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        // add by avinash for login with universe database password
        var hashPassoword = KeyPasswordSet(password);
        var query = _userRepository.Table;
        var user = await query.FirstOrDefaultAsync(user => user.EmailAddress == userName && user.Password == password || user.Password == hashPassoword && user.Deleted == false);

        return user;
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
    /// <summary>
    /// Decodes a custom encoded string by reversing the string, splitting it into three-digit ASCII values,
    /// and converting those values back to characters.
    /// </summary>
    /// <param name="sValue">The encoded string to decode.</param>
    /// <returns>The decoded string.</returns>
    public string KeyPasswordGet(string sValue)
    {

        string strR = "";
        char[] charArray = sValue.ToCharArray();
        Array.Reverse(charArray);
        sValue = new string(charArray);

        for (int i = 0; i < sValue.Length; i += 3)
        {
            if (i + 3 <= sValue.Length) // Ensure substring is within bounds
            {
                string strTemp = sValue.Substring(i, 3);
                if (int.TryParse(strTemp, out int intTemp)) // Safe parsing to avoid errors
                {
                    strR += (char)intTemp;
                }
                else
                {
                    // Handle unexpected non-numeric values gracefully
                    return "";
                }
            }
        }

        return strR;

    }
    public Task<List<long>> GetUserIdsByRoleIdAsync(long roleId)
    {
        return _userRepository.Table.Where(x => x.RoleId == roleId).Select(x => x.Id).ToListAsync();
    }
    /// <summary>
    /// Get user role by user identifier
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user role
    /// </returns>
    public async Task<UserRole> GetUserRoleAsync(long userId)
    {
        if (userId == 0)
            throw new ArgumentNullException(nameof(userId));

        var userRoleQuery = from u in _userRepository.Table
                            from ur in _userRoleRepository.Table.InnerJoin(ur => ur.Id == u.RoleId)
                            where u.Id == userId && u.Deleted == false && ur.Deleted == false
                            select ur;
        var query = await userRoleQuery.ToListAsync();
        return query.FirstOrDefault();
    }

    /// <summary>
    /// Insert user
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task InsertUserAsync(User user)
    {
        await _userRepository.InsertAsync(user);
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="user">User</param>
    /// <returns></returns>
    public async Task DeleteUserAsync(User user)
    {
        await _userRepository.DeleteAsync(user);
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <param name="searchText">Search code; null to load all records</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the genres
    /// </returns>
    public async Task<IPagedList<User>> GetAllUsersAsync(string searchText = null,
         int pageIndex = 0, int pageSize = int.MaxValue,
         bool showHidden = false)
    {
        try
        {
            var parameterList = new List<DataParameter>
                {
                     new DataParameter("@SearchText", searchText),
                    new DataParameter("@pageSize", pageSize),
                     new DataParameter("@startIndex", pageIndex)
                };

            parameterList.Add(new DataParameter("@TotalRows", 0) { Direction = ParameterDirection.Output });

            var jobs = await _userRepository.EntityFromSqlAsync(Procedures.SPGETALLUSERS, parameterList.ToArray());
            var totalCount = 0;
            var outputParameter = parameterList.FirstOrDefault(p => p.Direction == ParameterDirection.Output)?.Value ?? null;
            if (outputParameter != null && int.TryParse(outputParameter.ToString(), out int count))
                totalCount = count;
            var listModel = new PagedList<User>(jobs, pageSize, pageIndex, totalCount);
            return listModel;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error occurred During Retrieve the List Of Department !", ex);


        }
    }

    public async Task<IPagedList<User>> GetAllUsersAsync(string searchText = null, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var result = await _userRepository.GetAllPagedAsync(query =>
        {
            query = query.Where(x => !x.Deleted);
            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(d => d.Name.Contains(searchText) || d.MobileNumber.Contains(searchText) || d.EmailAddress.Contains(searchText));
            }

            // Order by Id descending
            query = query.OrderByDescending(d => d.Id);

            return query;
        }, pageIndex, pageSize);

        return result;
    }

    public async Task<IList<SelectListItem>> GetTrainerSelectListSAsync()
    {
        var query = await _userRepository.Table.Where(c=>c.RoleId!=2 && !c.Deleted).Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        }).ToListAsync();
        return query;
    }


    #endregion

    #region User Domain Mapping

    public async Task InsertUserDomainMappingAsync(UserDomainMapping entity)
    {
        await _userDomainMappingRepository.InsertAsync(entity);
    }


    /// <summary>
    /// Update user role
    /// </summary>
    /// <param name="userRole">userRole</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the userRole
    /// </returns>
    public async Task UpdateUserDomainMappingAsync(UserDomainMapping entity)
    {
        await _userDomainMappingRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// Delete user role
    /// </summary>
    /// <param name="userRole">userRole</param>
    /// <returns></returns>
    public async Task DeleteUserDomainMappingAsync(UserDomainMapping entity)
    {
        await _userDomainMappingRepository.DeleteAsync(entity);
    }

    public async Task<IList<UserDomainMapping>> GetUserDomainMappingListByUserIdAsync(long userId)
    {
        var query = _userDomainMappingRepository.Table.Where(e => e.UserId == userId);
        return await query.ToListAsync();
    }

    public async Task<IList<SelectListItem>> GetDomainWiseTrainerSelectListSAsync(long domainId)
    {
        return await _userRepository.Table
            .Join(_userDomainMappingRepository.Table,
                  u => u.Id,
                  d => d.UserId,
                  (u, d) => new { u, d })
            .Where(x => x.d.DomainId == domainId && x.u.RoleId != 2
                        && !x.u.Deleted && x.u.Active)
            .Select(x => new SelectListItem
            {
                Value = x.u.Id.ToString(),
                Text = x.u.Name
            })
            .ToListAsync();
    }

    #endregion

    #region Message Template

    public async Task<MessageTemplate> GetEmailTemplateByNameAsync(string name)
    {
        //emailTemplate
        var emailTemplate = await _messageTemplateRepository.Table.FirstOrDefaultAsync(x => x.Name == name);
        return emailTemplate;
    }

    public async Task<(bool IsValid, string Message, string Email, string UserType)> CheckResetTokenValidAsync(string resetToken)
    {
        if (string.IsNullOrWhiteSpace(resetToken))
            throw new ArgumentNullException(nameof(resetToken));

        // Check Users Table
        var user = await _userRepository.Table.FirstOrDefaultAsync(x => x.ResetToken == resetToken);
        if (user != null)
        {
            if (!user.ResetTokenExpiry.HasValue)
                return (false, "Token expiry not found.", null, null);

            if (user.ResetTokenExpiry.Value < DateTime.Now)
                return (false, "Reset link has expired.", null, null);

            return (true, "Link is valid.", user.EmailAddress, "User");
        }

        // Check Students Table
        var student = await _registrationMasterRepository.Table.FirstOrDefaultAsync(x => x.ResetToken == resetToken);
        if (student != null)
        {
            if (!student.ResetTokenExpiry.HasValue)
                return (false, "Token expiry not found.", null, null);

            if (student.ResetTokenExpiry.Value < DateTime.Now)
                return (false, "Reset link has expired.", null, null);

            return (true, "Link is valid.", student.Email, "Student");
        }

        return (false, "Token not found.", null, null);
    }

    public async Task<IList<User>> GetTrainerListById(List<long> ids = null)
    {
        if (ids?.Any() != true)
            return new List<User>();

        var query = await _userRepository.Table
            .Where(c => ids.Contains(c.Id))
            .Select(c => new User
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return query;
    }
    #endregion
}