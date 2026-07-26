namespace Project.Web.Framework.Models;

/// <summary>
/// Represents select list model
/// </summary>
public partial record SelectListModel : BaseModel
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Code of text
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
}
