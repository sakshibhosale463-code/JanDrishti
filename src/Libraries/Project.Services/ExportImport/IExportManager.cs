using Project.Core.Domain.Users;
using Project.Services.Candidate.EntityModel;

namespace Project.Services.ExportImport;

/// <summary>
/// Export manager interface
/// </summary>
public interface IExportManager
{
    /// <summary>
    /// Export users to XLSX
    /// </summary>
    /// <param name="users">Users</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<byte[]> ExportAllCandidateRegistrationDetailsToXlsxAsync(IList<RegistrationModel> registration);

    /// <summary>
    /// Export users to XLSX
    /// </summary>
    Task<byte[]> ExportAllUsersDetailToXlsxAsync(IList<User> users);
}