
namespace Project.Services.ExportImport
{
    /// <summary>
    /// Import manager interface
    /// </summary>
    public interface IImportManager
    {
        /// <summary>
        /// Import users from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task ImportUsersFromXlsxAsync(Stream stream);
        /// <summary>
        /// Import Discount from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>A task that represents the asynchronous operation</returns>
 

    }
}
