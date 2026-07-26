using Project.Core.Configuration;

namespace Project.Core.Domain.Catalog;

/// <summary>
/// Catalog settings
/// </summary>
public partial class CatalogSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether need create dropdown list for export
    /// </summary>
    public bool ExportImportUseDropdownlistsForAssociatedEntities { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the related entities need to be exported/imported using name
    /// </summary>
    public bool ExportImportRelatedEntitiesByName { get; set; }
}