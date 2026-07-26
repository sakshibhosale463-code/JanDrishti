using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Users;
using Project.Services.Candidate;
using Project.Services.Candidate.EntityModel;
using Project.Services.Catalog;
using Project.Services.Domain;
using Project.Services.ExportImport.Help;
using Project.Services.Localization;
using Project.Services.Users;

namespace Project.Services.ExportImport;

/// <summary>
/// Export manager interface
/// </summary>
public class ExportManager : IExportManager
{
    #region Fields

    protected readonly CatalogSettings _catalogSettings;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    private readonly IAttendanceService _attendanceService;
    private readonly IRegistrationMasterService _registrationMasterService;
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IDomainService _domainService;
    #endregion

    #region Constructor

    public ExportManager(CatalogSettings catalogSettings,
        ILanguageService languageService, IAttendanceService attendanceService,
        ILocalizationService localizationService,
        IRegistrationMasterService registrationMasterService,
        IUserService userService,
        IUserRoleService userRoleService,
        IDomainService domainService)
    {
        _catalogSettings = catalogSettings;
        _languageService = languageService;
        _attendanceService = attendanceService;
        _localizationService = localizationService;
        _registrationMasterService = registrationMasterService;
        _userService = userService;
        _userRoleService = userRoleService;
        _domainService = domainService;
    }

    #endregion

    #region Methods

    #region Users

    /// <summary>
    /// Export users to XLSX
    /// </summary>
    /// <param name="users">Users</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<byte[]> ExportAllCandidateRegistrationDetailsToXlsxAsync(IList<RegistrationModel> registration)
    {
        async Task<string> candidateCurrentStatusData(long id)
        {
            var status = await _registrationMasterService.GetCandidateTypeMastersByIdAsync(id);
            var currentStatusName = status?.name ?? string.Empty;
            return currentStatusName;
        }

        async Task<string> candidateInternShipTypeData(long id)
        {
            var enrollmentData = await _attendanceService.GetEnrollmentByRegisterIdAsync(id);
            var type = await _registrationMasterService.GetDomainMasterByIdAsync(enrollmentData.DomainId);
            var cource = type?.Name ?? string.Empty;

            return cource;
        }

        //property manager 
        var manager = new PropertyManager<RegistrationModel>(new[]
        {
            new PropertyByName<RegistrationModel>("Full Name", (p) => p.FullName),
            new PropertyByName<RegistrationModel>("Email", (p) => p.Email),
            new PropertyByName<RegistrationModel>("Mobile Number", (p) => p.MobileNumber),
            new PropertyByName<RegistrationModel>("Status", async item => (await candidateCurrentStatusData(item.CurrentStatus))),
            new PropertyByName<RegistrationModel>("College / University", (p) => p.CollageOrUniversityName),
            new PropertyByName<RegistrationModel>("Degree", (p) => p.CourceOrDegree),
            new PropertyByName<RegistrationModel>("CreatedOn", (p) => p.CreatedOn),
            new PropertyByName<RegistrationModel>("Passing Year", (p) => p.YearOfStudyPassingYear),
            new PropertyByName<RegistrationModel>("Weekend Session", (p) => p.AvailableForWeekendSessionName),
            new PropertyByName<RegistrationModel>("1-to-1 Session", (p) => p.AvailableForOneToOneSessionName),
            new PropertyByName<RegistrationModel>("Assessment Status", (p) => ((AssessmentStatusEnum)p.AssessmentStatus).ToString()),
            new PropertyByName<RegistrationModel>("Payment Status", (p) => ((PaymentStatusEnum)p.PaymentStatus).ToString()),
            new PropertyByName<RegistrationModel>("Paid Amount", (p) => p.PaidAmount),
            new PropertyByName<RegistrationModel>("Domain",  async item =>  (await candidateInternShipTypeData(item.Id))),

        }, _catalogSettings);

        return await manager.ExportToXlsxAsync(registration);
    }

    /// <summary>
    /// Export users to XLSX
    /// </summary>
    /// <param name="users">Users</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<byte[]> ExportAllUsersDetailToXlsxAsync(IList<User> users)
    {
        var roleIds = users.Select(x => x.RoleId).Distinct().ToList();
 
        // Load roles
        var roleDictionary = new Dictionary<long, string>();
        foreach (var roleId in roleIds)
        {
            var role = await _userRoleService.GetUserRoleByIdAsync(roleId);
            if (role != null && !roleDictionary.ContainsKey(roleId))
                roleDictionary.Add(roleId, role.Name ?? string.Empty);
        }

        // Load user domain mappings 
        var userDomainMappingDictionary = new Dictionary<long, List<long>>();

        foreach (var user in users)
        {
            var mappings = await _userService.GetUserDomainMappingListByUserIdAsync(user.Id);

            if (mappings != null && mappings.Any())
            {
                userDomainMappingDictionary[user.Id] =
                    mappings.Select(m => m.DomainId).ToList();
            }
        }

        // Get distinct domainIds
        var allDomainIds = userDomainMappingDictionary.SelectMany(x => x.Value).Distinct().ToList();

        // Load domains
        var domainDictionary = new Dictionary<long, string>();
        foreach (var domainId in allDomainIds)
        {
            var domain = await _domainService.GetDomainMasterByIdAsync(domainId);
            if (domain != null && !domainDictionary.ContainsKey(domainId))
                domainDictionary.Add(domainId, domain.Name ?? string.Empty);
        }

        // Create Property Manager
        var manager = new PropertyManager<User>(new[]
        {
        new PropertyByName<User>("Full Name", p => p.Name),
        new PropertyByName<User>("Email Address", p => p.EmailAddress),
        new PropertyByName<User>("Mobile Number", p => p.MobileNumber),
        new PropertyByName<User>("Role", p =>roleDictionary.ContainsKey(p.RoleId)
                ? roleDictionary[p.RoleId]: string.Empty),
        new PropertyByName<User>("Domains", p =>
        {
            if (!userDomainMappingDictionary.ContainsKey(p.Id))
                return string.Empty;

            var domainNames = userDomainMappingDictionary[p.Id]
                .Where(id => domainDictionary.ContainsKey(id))
                .Select(id => domainDictionary[id]);

            return string.Join(", ", domainNames);
        }),

        new PropertyByName<User>("Active", p => p.Active),
    }, _catalogSettings);

        return await manager.ExportToXlsxAsync(users);
    }



    #endregion
    #endregion
}
