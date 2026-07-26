using Project.Core;

namespace Project.Services.Common.DataModel;

/// <summary>
/// Represents select list data model
/// </summary>
public partial class SelectListDataModel : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of text
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the code
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// Gets or sets the value whether the item is checked or not
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Gets or sets the image
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Gets or sets the Referense id
    /// </summary>
    public int ReferenseId { get; set; }

    /// <summary>
    /// Gets or sets the Value
    /// </summary>
    public string Value { get; set; }
}
