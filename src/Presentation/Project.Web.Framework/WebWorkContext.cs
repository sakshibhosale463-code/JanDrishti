using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Project.Core;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Localization;
using Project.Core.Domain.Users;
using Project.Core.Http;
using Project.Services.Authentication;
using Project.Services.Localization;
using Project.Services.Users;

namespace Project.Web.Framework;

/// <summary>
/// Represents work context for web application
/// </summary>
public class WebWorkContext : IWorkContext
{
   
    #region Fields

    protected readonly ILanguageService _languageService;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;

    protected User _cachedUser;
    protected Language _cachedLanguage;
    protected RegistrationMaster _cachedStudent;

    #endregion

    #region Constructor

    public WebWorkContext(ILanguageService languageService,
        IHttpContextAccessor httpContextAccessor,
        IUserService userService,
        IAuthenticationService authenticationService)
    {
        _userService = userService;
        _languageService = languageService;
        _httpContextAccessor = httpContextAccessor;
        _authenticationService = authenticationService;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Set language culture cookie
    /// </summary>
    /// <param name="language">Language</param>
    protected virtual void SetLanguageCookie(Language language)
    {
        if (_httpContextAccessor.HttpContext?.Response.HasStarted ?? true)
            return;

        //delete current cookie value
        var cookieName = $"{ProjectCookieDefaults.Prefix}{ProjectCookieDefaults.CultureCookie}";
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

        if (string.IsNullOrEmpty(language?.LanguageCulture))
            return;

        //set new cookie value
        var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language.LanguageCulture));
        var options = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, value, options);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the current user
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<User> GetCurrentUserAsync()
    {
        //whether there is a cached value
        if (_cachedUser != null)
            return _cachedUser;

        await SetCurrentUserAsync();

        return _cachedUser;
    }

    /// <summary>
    /// Gets the current student
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<RegistrationMaster> GetCurrentStudentsync()
    {
        //whether there is a cached value
        if (_cachedUser != null)
            return _cachedStudent;

        await SetCurrentStudentAsync();

        return _cachedStudent;
    }

    public async Task<Language> GetWorkingLanguageAsync()
    {
        //whether there is a cached value
        if (_cachedLanguage != null)
            return _cachedLanguage;

        //if there are no languages for the current store try to get the first one regardless of the store
        var detectedLanguage = (await _languageService.GetAllLanguagesAsync()).FirstOrDefault();

        SetLanguageCookie(detectedLanguage);

        //cache the found language
        _cachedLanguage = detectedLanguage;

        return _cachedLanguage;
    }

    /// <summary>
    /// Sets the current user
    /// </summary>
    /// <param name="user">Current user</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task SetCurrentUserAsync(User user = null)
    {
        //try to get registered user
        if (user == null)
            user = await _authenticationService.GetAuthenticatedUserAsync();

        //cache the found user
        if (user != null && (!user.Deleted) && user.Active)
            _cachedUser = user;
    }

    /// <summary>
    /// Sets the current student
    /// </summary>
    /// <param name="student">Current student</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task SetCurrentStudentAsync(RegistrationMaster student = null)
    {
        //try to get registered user
        if (student == null)
            student = await _authenticationService.GetAuthenticatedStudentAsync();

        //cache the found user
        if (student != null && (!student.Deleted) && student.IsActive)
            _cachedStudent = student;
    }

    public Task SetWorkingLanguageAsync(Language language)
    {
        //set cookie
        SetLanguageCookie(language);

        //then reset the cached value
        _cachedLanguage = null;

        return Task.CompletedTask;
    }
    /// <summary>
    /// Gets the current user roles list
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<UserRole> GetCurrentUserRolesAsync()
    {
        var user = await GetCurrentUserAsync();
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        return await _userService.GetUserRoleAsync(user.Id);
    }


    /// <summary>
    /// Validates whether current user is admin or not
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<bool> IsCurrentUserIsAdmin()
    {
        var result = false;
        var userRole = await GetCurrentUserRolesAsync();
        if (userRole == null)
            return result;

        result = userRole.SystemName == ProjectUserDefaults.AdministratorsRoleName;
        return result;
    }
    #endregion
}
