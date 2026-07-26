using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Project.Core.Configuration;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Users;
using Project.Services.Candidate;
using Project.Services.Users;

namespace Project.Services.Authentication;

/// <summary>
/// Represents service using jwt middleware for the authentication
/// </summary>
public partial class JwtAuthenticationService : IAuthenticationService
{
    #region Fields

    private readonly AppSettings _appSettings;
    protected readonly IUserService _userService;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected User _cachedUser;
    protected RegistrationMaster _cachedStudent;


    protected IRegistrationMasterService _registrationMasterService;

    #endregion

    #region Constructor

    public JwtAuthenticationService(AppSettings appSettings,
        IUserService userService,
        IHttpContextAccessor httpContextAccessor, IRegistrationMasterService registrationMasterService)
    {
        _appSettings = appSettings;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _registrationMasterService = registrationMasterService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Generate token
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task<string> GenerateTokenAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        //create claims for user's username and email
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (!string.IsNullOrEmpty(user.Name))
            claims.Add(new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String, _appSettings.Get<JwtConfig>().Issuer));

        if (!string.IsNullOrEmpty(user.EmailAddress))
            claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress, ClaimValueTypes.Email, _appSettings.Get<JwtConfig>().Issuer));

        //create jwt
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Get<JwtConfig>().Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: _appSettings.Get<JwtConfig>().Issuer,
            audience: _appSettings.Get<JwtConfig>().Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(_appSettings.Get<JwtConfig>().TokenExpirationInMinitues)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return Task.FromResult(token);
    }


    /// <summary>
    /// Generate token
    /// </summary>
    /// <param name="RegisterMaster">student</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task<string> GenerateTokenForStudentAsync(RegistrationMaster student)
    {
        ArgumentNullException.ThrowIfNull(student);

        //create claims for user's username and email
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, student.Id.ToString())
        };

        if (!string.IsNullOrEmpty(student.FullName))
            claims.Add(new Claim(ClaimTypes.Name, student.FullName, ClaimValueTypes.String, _appSettings.Get<JwtConfig>().Issuer));

        if (!string.IsNullOrEmpty(student.Email))
            claims.Add(new Claim(ClaimTypes.Email, student.Email, ClaimValueTypes.Email, _appSettings.Get<JwtConfig>().Issuer));

        //create jwt
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Get<JwtConfig>().Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: _appSettings.Get<JwtConfig>().Issuer,
            audience: _appSettings.Get<JwtConfig>().Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(_appSettings.Get<JwtConfig>().TokenExpirationInMinitues)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return Task.FromResult(token);
    }

    /// <summary>
    /// Sign out
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task SignOutAsync()
    {
        //reset cached customer
        _cachedUser = null;

        //and sign out from the current authentication scheme
        await _httpContextAccessor.HttpContext.SignOutAsync(ProjectAuthenticationDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Get authenticated user
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    public async Task<User> GetAuthenticatedUserAsync()
    {
        //whether there is a cached user
        if (_cachedUser != null)
            return _cachedUser;

        //try to get authenticated user identity
        var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(ProjectAuthenticationDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
            return null;

        User user = null;
        if (authenticateResult.Principal.Claims.Any(claim => claim.Type == ClaimTypes.Email))
        {
            var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                && claim.Issuer.Equals(_appSettings.Get<JwtConfig>().Issuer, StringComparison.InvariantCultureIgnoreCase));
            //if (usernameClaim != null && string.TryParse(usernameClaim.Value, out string id))
            //    //    user = await _userService.GetUserByIdAsync(id);

                if (usernameClaim != null)
                user = await _userService.GetUserByEmailAsync(usernameClaim.Value);
                
        }

        //whether the found user is available
        if (user == null || !user.Active || user.Deleted)
            return null;

        //cache authenticated user
        _cachedUser = user;

        return _cachedUser;
    }


    /// <summary>
    /// Get authenticated student
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the student
    /// </returns>
    public async Task<RegistrationMaster> GetAuthenticatedStudentAsync()
    {
        //whether there is a cached user
        if (_cachedStudent != null)
            return _cachedStudent;

        //try to get authenticated user identity
        var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(ProjectAuthenticationDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
            return null;

        RegistrationMaster student = null;
        if (authenticateResult.Principal.Claims.Any(claim => claim.Type == ClaimTypes.Email))
        {
            var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                && claim.Issuer.Equals(_appSettings.Get<JwtConfig>().Issuer, StringComparison.InvariantCultureIgnoreCase));
            if (usernameClaim != null)
                student = await _registrationMasterService.GetRegistrationMasterByEmailIdAsync(usernameClaim.Value);
        }

        //whether the found user is available
        if (student == null || !student.IsActive || student.Deleted)
            return null;

        //cache authenticated user
        _cachedStudent = student;

        return _cachedStudent;
    }

    #endregion
}
