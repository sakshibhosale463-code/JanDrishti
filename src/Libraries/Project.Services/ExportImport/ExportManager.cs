using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Users;
using Project.Services.Catalog;
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
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    #endregion

    #region Constructor

    public ExportManager(CatalogSettings catalogSettings,
        ILanguageService languageService,
        ILocalizationService localizationService,
        IUserService userService,
        IUserRoleService userRoleService)
    {
        _catalogSettings = catalogSettings;
        _languageService = languageService;
        _localizationService = localizationService;
        _userService = userService;
        _userRoleService = userRoleService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Export users to XLSX
    /// </summary>
    /// <param name="users">Users</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    
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

        // Get distinct domainIds
        var allDomainIds = userDomainMappingDictionary.SelectMany(x => x.Value).Distinct().ToList();

        // Load domains
        var domainDictionary = new Dictionary<long, string>();
        
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
}
