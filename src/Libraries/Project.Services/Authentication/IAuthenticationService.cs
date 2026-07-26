using Project.Core.Domain.Candidate;
using Project.Core.Domain.Users;

namespace Project.Services.Authentication;

/// <summary>
/// Represents middleware that enables authentication
/// </summary>
public partial interface IAuthenticationService
{
    /// <summary>
    /// Generate token
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<string> GenerateTokenAsync(User user);


    /// <summary>
    /// Generate token
    /// </summary>
    /// <param name="RegisterMaster">student</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<string> GenerateTokenForStudentAsync(RegistrationMaster student);

    /// <summary>
    /// Sign out
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task SignOutAsync();

    /// <summary>
    /// Get authenticated user
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the user
    /// </returns>
    Task<User> GetAuthenticatedUserAsync();

    /// <summary>
    /// Get authenticated student
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the student
    /// </returns>
    Task<RegistrationMaster> GetAuthenticatedStudentAsync();
}
