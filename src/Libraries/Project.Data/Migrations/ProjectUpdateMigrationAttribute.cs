using Project.Core;

namespace Project.Data.Migrations;

/// <summary>
/// Attribute for update migration
/// </summary>
public partial class ProjectUpdateMigrationAttribute : ProjectMigrationAttribute
{
    /// <summary>
    /// Initializes a new instance of the NopUpdateMigrationAttribute class
    /// </summary>
    /// <param name="dateTime">The migration date time string to convert on version</param>
    /// <param name="nopVersion">nopCommerce full version</param>
    /// <param name="migrationType">The migration type</param>
    public ProjectUpdateMigrationAttribute(string dateTime, string nopVersion, UpdateMigrationType migrationType) :
        base(dateTime, nopVersion, migrationType, MigrationProcessType.Update)
    {
        ApplyInDbOnDebugMode = !_config.NopVersion.Equals(ProjectVersion.CURRENT_VERSION, StringComparison.CurrentCultureIgnoreCase);
    }
}