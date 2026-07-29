using Project.Core.Domain.Users;
namespace Project.Services.ExportImport;

/// <summary>
/// Export manager interface
/// </summary>
public interface IExportManager
{

    /// <summary>
    /// Export users to XLSX
    /// </summary>
    Task<byte[]> ExportAllUsersDetailToXlsxAsync(IList<User> users);
}