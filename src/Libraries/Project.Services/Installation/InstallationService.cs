using System.Globalization;
using Project.Core;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Localization;
using Project.Core.Domain.Master;
using Project.Core.Domain.Users;
using Project.Core.Infrastructure;
using Project.Data;
using Project.Services.Common;
using Project.Services.Localization;

namespace Project.Services.Installation;

/// <summary>
/// Installation service
/// </summary>
public partial class InstallationService : IInstallationService
{
    #region Fields

    private readonly IProjectDataProvider _dataProvider;
    private readonly IProjectFileProvider _fileProvider;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;

    #endregion

    #region Ctor

    public InstallationService(IProjectDataProvider dataProvider,
        IProjectFileProvider fileProvider,
        IRepository<User> userRepository,
        IRepository<UserRole> userRoleRepository)
    {
        _dataProvider = dataProvider;
        _fileProvider = fileProvider;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }

    #endregion

    #region Utilities

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task<T> InsertInstallationDataAsync<T>(T entity) where T : BaseEntity
    {
        return await _dataProvider.InsertEntityAsync(entity);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task InsertInstallationDataAsync<T>(params T[] entities) where T : BaseEntity
    {
        await _dataProvider.BulkInsertEntitiesAsync(entities);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task InsertInstallationDataAsync<T>(IList<T> entities) where T : BaseEntity
    {
        if (!entities.Any())
            return;

        await InsertInstallationDataAsync(entities.ToArray());
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task InstallLanguagesAsync(CultureInfo cultureInfo, RegionInfo regionInfo)
    {
        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

        var defaultCulture = new CultureInfo(ProjectCommonDefaults.DefaultLanguageCulture);
        var defaultLanguage = new Language
        {
            Name = defaultCulture.TwoLetterISOLanguageName.ToUpper(),
            LanguageCulture = defaultCulture.Name,
            UniqueSeoCode = defaultCulture.TwoLetterISOLanguageName,
            FlagImageFileName = $"{defaultCulture.Name.ToLower()[^2..]}.png",
            Rtl = defaultCulture.TextInfo.IsRightToLeft,
            Published = true,
            DisplayOrder = 1
        };
        await InsertInstallationDataAsync(defaultLanguage);

        //Install locale resources for default culture
        var directoryPath = _fileProvider.MapPath(ProjectInstallationDefaults.LocalizationResourcesPath);
        var pattern = $"*.{ProjectInstallationDefaults.LocalizationResourcesFileExtension}";
        foreach (var filePath in _fileProvider.EnumerateFiles(directoryPath, pattern))
        {
            using var streamReader = new StreamReader(filePath);
            await localizationService.ImportResourcesFromXmlAsync(defaultLanguage, streamReader);
        }

        if (cultureInfo == null || regionInfo == null || cultureInfo.Name == ProjectCommonDefaults.DefaultLanguageCulture)
            return;

        var language = new Language
        {
            Name = cultureInfo.TwoLetterISOLanguageName.ToUpper(),
            LanguageCulture = cultureInfo.Name,
            UniqueSeoCode = cultureInfo.TwoLetterISOLanguageName,
            FlagImageFileName = $"{regionInfo.TwoLetterISORegionName.ToLower()}.png",
            Rtl = cultureInfo.TextInfo.IsRightToLeft,
            Published = true,
            DisplayOrder = 2
        };
        await InsertInstallationDataAsync(language);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task InstallCurrenciesAsync(CultureInfo cultureInfo, RegionInfo regionInfo)
    {
        //set some currencies with a rate against the USD
        var defaultCurrencies = new List<string>() { "INR" };
        var currencies = new List<Currency>
            {
                new Currency
                {
                    Name = "Indian Rupee",
                    CurrencyCode = "INR",
                    Rate = 68.03M,
                    DisplayLocale = "en-IN",
                    CustomFormatting = string.Empty,
                    Published = false,
                    DisplayOrder = 12,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    RoundingType = RoundingType.Rounding001
                }
            };

        //set additional currency
        if (cultureInfo != null && regionInfo != null)
        {
            if (!defaultCurrencies.Contains(regionInfo.ISOCurrencySymbol))
            {
                currencies.Add(new Currency
                {
                    Name = regionInfo.CurrencyEnglishName,
                    CurrencyCode = regionInfo.ISOCurrencySymbol,
                    Rate = 1,
                    DisplayLocale = cultureInfo.Name,
                    CustomFormatting = string.Empty,
                    Published = true,
                    DisplayOrder = 0,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    RoundingType = RoundingType.Rounding001
                });
            }

            foreach (var currency in currencies.Where(currency => currency.CurrencyCode == regionInfo.ISOCurrencySymbol))
            {
                currency.Published = true;
                currency.DisplayOrder = 0;
            }
        }

        await InsertInstallationDataAsync(currencies);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task InstallUserRoleAsync()
    {
        await _userRoleRepository.InsertAsync(new UserRole()
        {
            Name = ProjectUserDefaults.AdministratorsRoleName,
            SystemName = ProjectUserDefaults.AdministratorsRoleName,
            Active = true,
            CreatedOnUtc = DateTime.Now,
            UpdatedOnUtc = DateTime.Now,
        });
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task InstallUsersAsync(string defaultUserEmail, string defaultUserPassword)
    {
        var userRoleQuery = _userRoleRepository.Table.Where(u => u.Name.Equals(ProjectUserDefaults.AdministratorsRoleName));
        var userRole = await userRoleQuery.FirstOrDefaultAsync();

        var user = new User()
        {
            Active = true,
            RoleId = userRole.Id,
            Name = defaultUserEmail,
            MobileNumber = string.Empty,
            CreatedOnUtc = DateTime.Now,
            UpdatedOnUtc = DateTime.Now,
            EmailAddress = defaultUserEmail,
            Password=defaultUserPassword

        };
        await _userRepository.InsertAsync(user);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Install required data
    /// </summary>
    /// <param name="defaultUserEmail">Default user email</param>
    /// <param name="defaultUserPassword">Default user password</param>
    /// <param name="regionInfo">RegionInfo</param>
    /// <param name="cultureInfo">CultureInfo</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InstallRequiredDataAsync(string defaultUserEmail, string defaultUserPassword,
        RegionInfo regionInfo, CultureInfo cultureInfo)
    {
        await InstallLanguagesAsync(cultureInfo, regionInfo);
        await InstallCurrenciesAsync(cultureInfo, regionInfo);
        await InstallUserRoleAsync();
        await InstallUsersAsync(defaultUserEmail, defaultUserPassword);
    }

    /// <summary>
    /// Install sample data
    /// </summary>
    /// <param name="defaultUserEmail">Default user email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual Task InstallSampleDataAsync(string defaultUserEmail)
    {
        return Task.CompletedTask;
    }

    #endregion
}
